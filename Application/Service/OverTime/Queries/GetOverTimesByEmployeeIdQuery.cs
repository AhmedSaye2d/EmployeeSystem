using Application.Dtos.Response.OverTime;
using MediatR;

namespace Application.Service.OverTime.Queries
{
    public record GetOverTimesByEmployeeIdQuery(int EmployeeId) : IRequest<List<OverTimeResponse>>;
}
