using Application.Dtos.Response;
using MediatR;

namespace Application.Service.Payroll.Commands
{
    public record PayPayrollCommand(int Id, DateTime? PaymentDate = null) : IRequest<GeneralResponse>;
}
