using MediatR;
using Application.Dtos.Response.Country;

namespace Application.Service.Country.Queries
{
    public record GetCountriesQuery : IRequest<List<CountryResponse>>;
}
