using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.Payroll.Commands;
using Domain.Entities.Enum;
using MediatR;

namespace Infrastructure.Handler.PayrollHandler
{
    public class DeletePayrollDraftHandler : IRequestHandler<DeletePayrollDraftCommand, GeneralResponse>
    {
        private readonly IPayrollRepository _payrollRepository;

        public DeletePayrollDraftHandler(IPayrollRepository payrollRepository)
        {
            _payrollRepository = payrollRepository;
        }

        public async Task<GeneralResponse> Handle(
            DeletePayrollDraftCommand request,
            CancellationToken cancellationToken)
        {
            var payroll = await _payrollRepository.GetByIdAsync(request.Id);
            if (payroll is null)
                return new GeneralResponse(false, $"Payroll with ID {request.Id} not found.");

            // قاعدة الأمان الصارمة: لا يمكن حذف أي مسير معتمد أو تم صرفه
            if (payroll.Status != PayrollStatus.Draft)
                return new GeneralResponse(false, $"Cannot delete Payroll #{payroll.Id} because it is already {payroll.Status}. Only Draft payrolls can be deleted.");

            return await _payrollRepository.DeleteAsync(request.Id);
        }
    }
}
