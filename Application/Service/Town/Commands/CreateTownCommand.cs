using MediatR;
using Application.Dtos.Request.Town;
using Application.Dtos.Response;

namespace Application.Service.Town.Commands
{
    public record CreateTownCommand(AddTownRequest Town) : IRequest<GeneralResponse>;
}
