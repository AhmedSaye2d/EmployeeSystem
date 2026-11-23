using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.Payroll.Commands;
using Domain.Entities.Enum;
using MediatR;

namespace Infrastructure.Handler.PayrollHandler
{
    public class ApprovePayrollHandler : IRequestHandler<ApprovePayrollCommand, GeneralResponse>
    {
        private readonly IPayrollRepository _payrollRepository;

        public ApprovePayrollHandler(IPayrollRepository payrollRepository)
        {
            _payrollRepository = payrollRepository;
        }

        public async Task<GeneralResponse> Handle(
            ApprovePayrollCommand request,
            CancellationToken cancellationToken)
        {
            var payroll = await _payrollRepository.GetByIdAsync(request.Id);
            if (payroll is null)
                return new GeneralResponse(false, $"Payroll with ID {request.Id} not found.");

            if (payroll.Status == PayrollStatus.Approved)
                return new GeneralResponse(false, $"Payroll #{payroll.Id} is already approved.");

            if (payroll.Status == PayrollStatus.Paid)
                return new GeneralResponse(false, $"Payroll #{payroll.Id} is already paid.");

            payroll.Status = PayrollStatus.Approved;
            payroll.ApprovedAt = DateTime.UtcNow;

            var result = await _payrollRepository.UpdateAsync(payroll);
            if (result.Flag)
            {
                return new GeneralResponse(true, $"Payroll #{payroll.Id} approved successfully.");
            }

            return result;
        }
    }
}
