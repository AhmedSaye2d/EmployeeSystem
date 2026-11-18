using MediatR;
using Application.Dtos.Response.Department;

namespace Application.Service.Department.Queries
{
    public record GetDepartmentByIdQuery(int Id) : IRequest<DepartmentResponse?>;
}
