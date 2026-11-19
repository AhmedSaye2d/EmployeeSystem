using MediatR;
using Application.Dtos.Response.OverTimeType;

namespace Application.Service.OverTimeType.Queries
{
    public record GetOverTimeTypesQuery : IRequest<List<OverTimeTypeResponse>>;
}
