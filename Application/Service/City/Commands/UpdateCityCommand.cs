using MediatR;
using Application.Dtos.Request.City;
using Application.Dtos.Response;

namespace Application.Service.City.Commands
{
    public record UpdateCityCommand(UpdateCityRequest City) : IRequest<GeneralResponse>;
}
