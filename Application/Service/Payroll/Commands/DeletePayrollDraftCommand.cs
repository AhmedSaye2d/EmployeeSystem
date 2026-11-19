using Application.Dtos.Response;
using MediatR;

namespace Application.Service.Payroll.Commands
{
    public record DeletePayrollDraftCommand(int Id) : IRequest<GeneralResponse>;
}
