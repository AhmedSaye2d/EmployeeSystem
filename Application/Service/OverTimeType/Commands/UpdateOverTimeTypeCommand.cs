using MediatR;
using Application.Dtos.Request.OverTimeType;
using Application.Dtos.Response;

namespace Application.Service.OverTimeType.Commands
{
    public record UpdateOverTimeTypeCommand(UpdateOverTimeTypeRequest OverTimeType) : IRequest<GeneralResponse>;
}
