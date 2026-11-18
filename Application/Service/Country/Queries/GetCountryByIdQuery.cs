using MediatR;
using Application.Dtos.Response.Country;

namespace Application.Service.Country.Queries
{
    public record GetCountryByIdQuery(int Id) : IRequest<CountryResponse?>;
}
