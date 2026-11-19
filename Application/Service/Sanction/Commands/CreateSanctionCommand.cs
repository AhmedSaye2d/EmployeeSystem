using MediatR;
using Application.Dtos.Request.Sanction;
using Application.Dtos.Response;

namespace Application.Service.Sanction.Commands
{
    public record CreateSanctionCommand(AddSanctionRequest Sanction) : IRequest<GeneralResponse>;
}
