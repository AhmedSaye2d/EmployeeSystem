using MediatR;
using Application.Dtos.Response.OverTime;

namespace Application.Service.OverTime.Queries
{
    public record GetOverTimesQuery : IRequest<List<OverTimeResponse>>;
}
