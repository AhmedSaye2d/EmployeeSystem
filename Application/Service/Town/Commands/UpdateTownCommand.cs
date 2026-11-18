using MediatR;
using Application.Dtos.Request.Town;
using Application.Dtos.Response;

namespace Application.Service.Town.Commands
{
    public record UpdateTownCommand(UpdateTownRequest Town) : IRequest<GeneralResponse>;
}
