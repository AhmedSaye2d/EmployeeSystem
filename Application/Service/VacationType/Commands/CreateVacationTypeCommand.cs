using MediatR;
using Application.Dtos.Request.VacationType;
using Application.Dtos.Response;

namespace Application.Service.VacationType.Commands
{
    public record CreateVacationTypeCommand(AddVacationTypeRequest VacationType) : IRequest<GeneralResponse>;
}
