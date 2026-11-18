using MediatR;
using Application.Dtos.Request.GeneralDepartment;
using Application.Dtos.Response;

namespace Application.Service.GeneralDepartment.Commands
{
    public record CreateGeneralDepartmentCommand(AddGeneralDepartmentRequest GeneralDepartment) : IRequest<GeneralResponse>;
}
