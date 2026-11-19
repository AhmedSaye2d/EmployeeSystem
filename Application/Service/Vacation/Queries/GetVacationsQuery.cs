using MediatR;
using Application.Dtos.Response.Vacation;

namespace Application.Service.Vacation.Queries
{
    public record GetVacationsQuery : IRequest<List<VacationResponse>>;
}
