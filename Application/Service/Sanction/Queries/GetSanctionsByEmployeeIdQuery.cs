using MediatR;
using Application.Dtos.Response.Sanction;

namespace Application.Service.Sanction.Queries
{
    public record GetSanctionsByEmployeeIdQuery(int EmployeeId) : IRequest<List<SanctionResponse>>;
}
