using MediatR;
using Application.Dtos.Response;

namespace Application.Service.Department.Commands
{
    public record DeleteDepartmentCommand(int Id) : IRequest<GeneralResponse>;
}
