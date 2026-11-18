using Application.Dtos.Response.Employee;
using MediatR;

namespace Application.Service.Employee.Queries
{
    public record GetEmployeeByIdQuery(int Id) : IRequest<EmployeeDetailsResponse?>;
}
