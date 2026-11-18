using MediatR;
using Application.Dtos.Response;

namespace Application.Service.City.Commands
{
    public record DeleteCityCommand(int Id) : IRequest<GeneralResponse>;
}
