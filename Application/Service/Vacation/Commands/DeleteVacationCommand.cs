using MediatR;
using Application.Dtos.Response;

namespace Application.Service.Vacation.Commands
{
    public record DeleteVacationCommand(int Id) : IRequest<GeneralResponse>;
}
