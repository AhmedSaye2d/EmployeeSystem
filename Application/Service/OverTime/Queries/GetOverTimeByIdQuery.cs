using MediatR;
using Application.Dtos.Response.OverTime;

namespace Application.Service.OverTime.Queries
{
    public record GetOverTimeByIdQuery(int Id) : IRequest<OverTimeResponse?>;
}
