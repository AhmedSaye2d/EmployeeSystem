using Application.Dtos.Request.Vacation;
using Application.Dtos.Response;
using MediatR;

namespace Application.Service.Vacation.Commands
{
    public record ReviewVacationCommand(ReviewVacationRequest Request, string Reviewer) : IRequest<GeneralResponse>;
}
