using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.Payroll.Commands;
using Domain.Entities.Enum;
using MediatR;

namespace Infrastructure.Handler.PayrollHandler
{
    public class PayPayrollHandler : IRequestHandler<PayPayrollCommand, GeneralResponse>
    {
        private readonly IPayrollRepository _payrollRepository;

        public PayPayrollHandler(IPayrollRepository payrollRepository)
        {
            _payrollRepository = payrollRepository;
        }

        public async Task<GeneralResponse> Handle(
            PayPayrollCommand request,
            CancellationToken cancellationToken)
        {
            var payroll = await _payrollRepository.GetByIdAsync(request.Id);
            if (payroll is null)
                return new GeneralResponse(false, $"Payroll with ID {request.Id} not found.");

            if (payroll.Status == PayrollStatus.Paid)
                return new GeneralResponse(false, $"Payroll #{payroll.Id} has already been paid on {payroll.PaymentDate:yyyy-MM-dd}.");

            payroll.Status = PayrollStatus.Paid;
            payroll.PaymentDate = request.PaymentDate ?? DateTime.UtcNow;

            var result = await _payrollRepository.UpdateAsync(payroll);
            if (result.Flag)
            {
                return new GeneralResponse(true, $"Payroll #{payroll.Id} marked as Paid successfully on {payroll.PaymentDate:yyyy-MM-dd}.");
            }

            return result;
        }
    }
}
