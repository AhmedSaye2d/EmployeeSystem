using MediatR;
using Application.Dtos.Request.Department;
using Application.Dtos.Response;

namespace Application.Service.Department.Commands
{
    public record CreateDepartmentCommand(AddDepartmentRequest Department) : IRequest<GeneralResponse>;
}
