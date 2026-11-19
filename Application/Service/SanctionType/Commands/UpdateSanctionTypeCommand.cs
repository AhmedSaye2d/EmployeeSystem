using MediatR;
using Application.Dtos.Request.SanctionType;
using Application.Dtos.Response;

namespace Application.Service.SanctionType.Commands
{
    public record UpdateSanctionTypeCommand(UpdateSanctionTypeRequest SanctionType) : IRequest<GeneralResponse>;
}
