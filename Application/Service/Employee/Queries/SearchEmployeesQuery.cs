using MediatR;
using Application.Dtos.Response.Employee;

namespace Application.Service.Employee.Queries
{
    public record SearchEmployeesQuery(string SearchTerm) : IRequest<List<EmployeeResponse>>;
}
