using MediatR;
using Application.Dtos.Response.City;

namespace Application.Service.City.Queries
{
    public record GetCitiesQuery : IRequest<List<CityResponse>>;
}
