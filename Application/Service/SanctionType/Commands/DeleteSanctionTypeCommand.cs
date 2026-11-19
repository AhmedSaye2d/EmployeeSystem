using MediatR;
using Application.Dtos.Response;

namespace Application.Service.SanctionType.Commands
{
    public record DeleteSanctionTypeCommand(int Id) : IRequest<GeneralResponse>;
}
