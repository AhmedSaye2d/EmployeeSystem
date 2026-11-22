using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.Contracts.Events;
using Application.Service.Contracts.Logging;
using Application.Service.Vacation.Commands;
using Domain.Entities.Enum;
using MassTransit;
using MediatR;

namespace Infrastructure.Handler.VacationHandler
{
    public class ReviewVacationHandler(IVacationRepository vacationRepository,
        IPublishEndpoint publishEndpoint,
        IAppLogger<ReviewVacationHandler> logger) : IRequestHandler<ReviewVacationCommand, GeneralResponse>

    {
        public async Task<GeneralResponse> Handle(
            ReviewVacationCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Request is null)
                return new GeneralResponse(false, "Review request is required.");

            if (request.Request.Decision != VacationStatus.Approved && request.Request.Decision != VacationStatus.Rejected)
                return new GeneralResponse(false, "Invalid decision. Only Approved or Rejected allowed.");

            // 1. جلب الإجازة
            var vacation = await vacationRepository.GetByIdAsync(request.Request.VacationId);
            if (vacation is null)
                return new GeneralResponse(false, "Vacation request not found.");

            // 2. التحقق من أن حالة الإجازة ما زالت Pending
            if (vacation.Status != VacationStatus.Pending)
                return new GeneralResponse(false, $"Vacation has already been decided as: {vacation.Status}.");

            // 3. تحديث حالة الإجازة وتوثيق من اتخذ القرار وتاريخه
            var decisionDate = DateTime.UtcNow;
            vacation.Status = request.Request.Decision;
            vacation.ApprovedOrRejectedBy = request.Reviewer;
            vacation.DecisionDate = decisionDate;
            vacation.DecisionReason = request.Request.Reason;

            var employeeName = vacation.Employee?.Name ?? "Employee";
            var employeeEmail = vacation.Employee?.Email ?? string.Empty;
            var vacationTypeName = vacation.VacationType?.Name ?? "Vacation";

            // 4. تجهيز ونشر الحدث إلى Outbox وفق القرار مع تضمين EventId فريد لضمان Idempotency
            var eventId = Guid.NewGuid();
            if (request.Request.Decision == VacationStatus.Approved)
            {
                await publishEndpoint.Publish(new VacationApprovedEvent(
                    EventId: eventId,
                    VacationId: vacation.Id,
                    EmployeeId: vacation.EmployeeId,
                    EmployeeName: employeeName,
                    EmployeeEmail: employeeEmail,
                    VacationTypeName: vacationTypeName,
                    StartDate: vacation.StartData,
                    EndDate: vacation.EndData,
                    NumberOfDays: vacation.NumberOfDays,
                    ApprovedBy: request.Reviewer,
                    ApprovedAt: decisionDate
                ), cancellationToken);
            }
            else
            {
                await publishEndpoint.Publish(new VacationRejectedEvent(
                    EventId: eventId,
                    VacationId: vacation.Id,
                    EmployeeId: vacation.EmployeeId,
                    EmployeeName: employeeName,
                    EmployeeEmail: employeeEmail,
                    VacationTypeName: vacationTypeName,
                    StartDate: vacation.StartData,
                    EndDate: vacation.EndData,
                    NumberOfDays: vacation.NumberOfDays,
                    RejectedBy: request.Reviewer,
                    RejectedAt: decisionDate,
                    RejectionReason: request.Request.Reason
                ), cancellationToken);
            }

            // 5. حفظ التعديل ورسالة الـ Outbox معاً في نفس المعاملة
            var result = await vacationRepository.UpdateAsync(vacation);
            if (result.Flag)
            {
                logger.LogInformation($"Vacation #{vacation.Id} has been {vacation.Status} by {request.Reviewer} and event published to Outbox.");
                return new GeneralResponse(true, $"Vacation has been {vacation.Status} successfully.");
            }

            return result;
        }
    }
}
