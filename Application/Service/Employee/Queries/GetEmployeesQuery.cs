using MediatR;
using Application.Dtos.Response.Employee;

namespace Application.Service.Employee.Queries
{
    public record GetEmployeesQuery : IRequest<List<EmployeeResponse>>;
}
