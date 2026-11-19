using Application.Dtos.Response.Vacation;
using MediatR;

namespace Application.Service.Vacation.Queries
{
    public record GetPendingVacationsQuery : IRequest<IEnumerable<VacationResponse>>;
}
