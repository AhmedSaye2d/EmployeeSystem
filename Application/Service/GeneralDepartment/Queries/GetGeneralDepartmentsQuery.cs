using MediatR;
using Application.Dtos.Response.GeneralDepartment;

namespace Application.Service.GeneralDepartment.Queries
{
    public record GetGeneralDepartmentsQuery : IRequest<List<GeneralDepartmentResponse>>;
}
