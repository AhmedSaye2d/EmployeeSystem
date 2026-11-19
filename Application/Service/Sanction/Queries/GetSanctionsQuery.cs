using MediatR;
using Application.Dtos.Response.Sanction;

namespace Application.Service.Sanction.Queries
{
    public record GetSanctionsQuery : IRequest<List<SanctionResponse>>;
}
