using MediatR;
using Application.Dtos.Response;

namespace Application.Service.GeneralDepartment.Commands
{
    public record DeleteGeneralDepartmentCommand(int Id) : IRequest<GeneralResponse>;
}
