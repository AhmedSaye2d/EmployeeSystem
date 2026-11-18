using MediatR;
using Application.Dtos.Request.Country;
using Application.Dtos.Response;

namespace Application.Service.Country.Commands
{
    public record UpdateCountryCommand(UpdateCountryRequest Country) : IRequest<GeneralResponse>;
}
