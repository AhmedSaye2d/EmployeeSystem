using MediatR;
using Application.Dtos.Request.GeneralDepartment;
using Application.Dtos.Response;

namespace Application.Service.GeneralDepartment.Commands
{
    public record UpdateGeneralDepartmentCommand(UpdateGeneralDepartmentRequest GeneralDepartment) : IRequest<GeneralResponse>;
}
