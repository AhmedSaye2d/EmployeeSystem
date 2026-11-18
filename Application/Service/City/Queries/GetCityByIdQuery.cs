using MediatR;
using Application.Dtos.Response.City;

namespace Application.Service.City.Queries
{
    public record GetCityByIdQuery(int Id) : IRequest<CityResponse?>;
}
