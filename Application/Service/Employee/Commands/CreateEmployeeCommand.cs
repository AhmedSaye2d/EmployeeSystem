using MediatR;
using Application.Dtos.Request.Employee;
using Application.Dtos.Response;

namespace Application.Service.Employee.Commands
{
    public record CreateEmployeeCommand(AddEmployeeRequest Employee) : IRequest<GeneralResponse>;
}
