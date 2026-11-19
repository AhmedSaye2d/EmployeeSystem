using MediatR;
using Application.Dtos.Response;

namespace Application.Service.OverTimeType.Commands
{
    public record DeleteOverTimeTypeCommand(int Id) : IRequest<GeneralResponse>;
}
