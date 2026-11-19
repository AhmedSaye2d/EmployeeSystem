using MediatR;
using Application.Dtos.Request.VacationType;
using Application.Dtos.Response;

namespace Application.Service.VacationType.Commands
{
    public record UpdateVacationTypeCommand(UpdateVacationTypeRequest VacationType) : IRequest<GeneralResponse>;
}
