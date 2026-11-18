using MediatR;
using Application.Dtos.Response;

namespace Application.Service.Branch.Commands
{
    public record DeleteBranchCommand(int Id) : IRequest<GeneralResponse>;
}
