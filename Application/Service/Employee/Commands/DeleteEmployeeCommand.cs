using MediatR;
using Application.Dtos.Response;

namespace Application.Service.Employee.Commands
{
    public record DeleteEmployeeCommand(int Id) : IRequest<GeneralResponse>;
}
