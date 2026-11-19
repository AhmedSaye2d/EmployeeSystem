using Application.Dtos.Response;
using MediatR;

namespace Application.Service.Payroll.Commands
{
    public record ApprovePayrollCommand(int Id) : IRequest<GeneralResponse>;
}
