using MediatR;
using Application.Dtos.Response.GeneralDepartment;

namespace Application.Service.GeneralDepartment.Queries
{
    public record GetGeneralDepartmentByIdQuery(int Id) : IRequest<GeneralDepartmentResponse?>;
}
