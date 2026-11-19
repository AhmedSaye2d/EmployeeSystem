using Application.Dtos.Request.Payroll;
using Application.Dtos.Response;
using MediatR;

namespace Application.Service.Payroll.Commands
{
    public record CreatePayrollCommand(CreatePayrollRequest Request) : IRequest<GeneralResponse>;
}
