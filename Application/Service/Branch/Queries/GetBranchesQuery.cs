using MediatR;
using Application.Dtos.Response.Branch;

namespace Application.Service.Branch.Queries
{
    public record GetBranchesQuery : IRequest<List<BranchResponse>>;
}
