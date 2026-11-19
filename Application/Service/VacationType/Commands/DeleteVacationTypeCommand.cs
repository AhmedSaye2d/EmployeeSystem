using MediatR;
using Application.Dtos.Response;

namespace Application.Service.VacationType.Commands
{
    public record DeleteVacationTypeCommand(int Id) : IRequest<GeneralResponse>;
}
