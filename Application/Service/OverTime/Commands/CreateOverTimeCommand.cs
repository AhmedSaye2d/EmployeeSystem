using MediatR;
using Application.Dtos.Request.OverTime;
using Application.Dtos.Response;

namespace Application.Service.OverTime.Commands
{
    public record CreateOverTimeCommand(AddOverTimeRequest OverTime) : IRequest<GeneralResponse>;
}
