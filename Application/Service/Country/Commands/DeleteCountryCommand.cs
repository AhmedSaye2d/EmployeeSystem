using MediatR;
using Application.Dtos.Response;

namespace Application.Service.Country.Commands
{
    public record DeleteCountryCommand(int Id) : IRequest<GeneralResponse>;
}
