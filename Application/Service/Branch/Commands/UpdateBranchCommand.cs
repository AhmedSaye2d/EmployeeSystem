using MediatR;
using Application.Dtos.Request.Branch;
using Application.Dtos.Response;

namespace Application.Service.Branch.Commands
{
    public record UpdateBranchCommand(UpdateBranchRequest Branch) : IRequest<GeneralResponse>;
}
