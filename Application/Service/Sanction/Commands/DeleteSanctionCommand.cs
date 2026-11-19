using MediatR;
using Application.Dtos.Response;

namespace Application.Service.Sanction.Commands
{
    public record DeleteSanctionCommand(int Id) : IRequest<GeneralResponse>;
}
