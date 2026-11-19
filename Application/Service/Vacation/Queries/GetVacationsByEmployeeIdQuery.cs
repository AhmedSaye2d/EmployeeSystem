using MediatR;
using Application.Dtos.Response.Vacation;

namespace Application.Service.Vacation.Queries
{
    public record GetVacationsByEmployeeIdQuery(int EmployeeId) : IRequest<List<VacationResponse>>;
}
