using Application.Dtos.Response;
using Application.Service.Branch.Commands;
using Application.Service.Contracts;
using DomainBranch = Domain.Entities.Branch;
using MediatR;

namespace Infrastructure.Handler.BranchHandler
{
    public class DeleteBranchHandler : IRequestHandler<DeleteBranchCommand, GeneralResponse>
    {
        private readonly IGeneralRepositoryInterface<DomainBranch> _branchRepository;

        public DeleteBranchHandler(IGeneralRepositoryInterface<DomainBranch> branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task<GeneralResponse> Handle(
            DeleteBranchCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return new GeneralResponse(false, "Invalid branch ID.");

            return await _branchRepository.DeleteAsync(request.Id);
        }
    }
}
