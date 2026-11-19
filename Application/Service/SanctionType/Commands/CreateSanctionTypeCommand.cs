using MediatR;
using Application.Dtos.Request.SanctionType;
using Application.Dtos.Response;

namespace Application.Service.SanctionType.Commands
{
    public record CreateSanctionTypeCommand(AddSanctionTypeRequest SanctionType) : IRequest<GeneralResponse>;
}
