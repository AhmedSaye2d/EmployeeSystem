using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.Payroll.Commands;
using Domain.Entities.Enum;
using MediatR;

namespace Infrastructure.Handler.PayrollHandler
{
    public class UpdatePayrollDraftHandler : IRequestHandler<UpdatePayrollDraftCommand, GeneralResponse>
    {
        private readonly IPayrollRepository _payrollRepository;

        public UpdatePayrollDraftHandler(IPayrollRepository payrollRepository)
        {
            _payrollRepository = payrollRepository;
        }

        public async Task<GeneralResponse> Handle(
            UpdatePayrollDraftCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Request is null)
                return new GeneralResponse(false, "Update request data is required.");

            var payroll = await _payrollRepository.GetByIdAsync(request.Request.Id);
            if (payroll is null)
                return new GeneralResponse(false, $"Payroll with ID {request.Request.Id} not found.");

            // قاعدة الأمان: لا يمكن تعديل مسير تم اعتماده أو صرفه للحفاظ على السجلات التاريخية
            if (payroll.Status != PayrollStatus.Draft)
                return new GeneralResponse(false, $"Cannot update Payroll #{payroll.Id} because it is already {payroll.Status}. Only Draft payrolls can be modified.");

            // تحديث البدلات وإعادة احتساب الصافي
            payroll.Allowances = request.Request.Allowances;
            decimal gross = payroll.BasicSalary + payroll.Allowances + payroll.OverTimeAmount;
            payroll.NetSalary = gross - payroll.Deductions;

            if (request.Request.Notes != null)
            {
                payroll.Notes = request.Request.Notes;
            }

            return await _payrollRepository.UpdateAsync(payroll);
        }
    }
}
