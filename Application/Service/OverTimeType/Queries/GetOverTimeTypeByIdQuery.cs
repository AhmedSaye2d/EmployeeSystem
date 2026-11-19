using MediatR;
using Application.Dtos.Response.OverTimeType;

namespace Application.Service.OverTimeType.Queries
{
    public record GetOverTimeTypeByIdQuery(int Id) : IRequest<OverTimeTypeResponse?>;
}
