using MediatR;
using Application.Dtos.Response.Sanction;

namespace Application.Service.Sanction.Queries
{
    public record GetSanctionByIdQuery(int Id) : IRequest<SanctionResponse?>;
}
