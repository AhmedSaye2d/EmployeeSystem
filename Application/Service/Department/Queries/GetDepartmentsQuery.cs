using MediatR;
using Application.Dtos.Response.Department;

namespace Application.Service.Department.Queries
{
    public record GetDepartmentsQuery : IRequest<List<DepartmentResponse>>;
}
