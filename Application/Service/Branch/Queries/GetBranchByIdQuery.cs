using MediatR;
using Application.Dtos.Response.Branch;

namespace Application.Service.Branch.Queries
{
    public record GetBranchByIdQuery(int Id) : IRequest<BranchResponse?>;
}
