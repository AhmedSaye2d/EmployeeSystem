using MediatR;
using Application.Dtos.Response.Town;

namespace Application.Service.Town.Queries
{
    public record GetTownsQuery : IRequest<List<TownResponse>>;
}
