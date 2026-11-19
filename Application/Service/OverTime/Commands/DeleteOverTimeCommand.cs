using MediatR;
using Application.Dtos.Response;

namespace Application.Service.OverTime.Commands
{
    public record DeleteOverTimeCommand(int Id) : IRequest<GeneralResponse>;
}
