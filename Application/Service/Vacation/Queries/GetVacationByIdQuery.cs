using MediatR;
using Application.Dtos.Response.Vacation;

namespace Application.Service.Vacation.Queries
{
    public record GetVacationByIdQuery(int Id) : IRequest<VacationResponse?>;
}
