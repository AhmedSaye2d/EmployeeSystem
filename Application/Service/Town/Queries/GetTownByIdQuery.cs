using MediatR;
using Application.Dtos.Response.Town;

namespace Application.Service.Town.Queries
{
    public record GetTownByIdQuery(int Id) : IRequest<TownResponse?>;
}
