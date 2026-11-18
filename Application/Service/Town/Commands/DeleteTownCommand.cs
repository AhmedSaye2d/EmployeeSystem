using MediatR;
using Application.Dtos.Response;

namespace Application.Service.Town.Commands
{
    public record DeleteTownCommand(int Id) : IRequest<GeneralResponse>;
}
