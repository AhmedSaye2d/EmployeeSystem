using MediatR;
using Application.Dtos.Request.OverTime;
using Application.Dtos.Response;

namespace Application.Service.OverTime.Commands
{
    public record UpdateOverTimeCommand(UpdateOverTimeRequest OverTime) : IRequest<GeneralResponse>;
}
