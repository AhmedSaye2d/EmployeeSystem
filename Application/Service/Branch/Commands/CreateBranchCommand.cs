using MediatR;
using Application.Dtos.Request.Branch;
using Application.Dtos.Response;

namespace Application.Service.Branch.Commands
{
    public record CreateBranchCommand(AddBranchRequest Branch) : IRequest<GeneralResponse>;
}
