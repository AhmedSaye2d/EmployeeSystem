using MediatR;
using Application.Dtos.Request.Vacation;
using Application.Dtos.Response;

namespace Application.Service.Vacation.Commands
{
    public record CreateVacationCommand(AddVacationRequest Vacation) : IRequest<GeneralResponse>;
}
