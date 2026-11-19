using MediatR;
using Application.Dtos.Response.SanctionType;

namespace Application.Service.SanctionType.Queries
{
    public record GetSanctionTypeByIdQuery(int Id) : IRequest<SanctionTypeResponse?>;
}
