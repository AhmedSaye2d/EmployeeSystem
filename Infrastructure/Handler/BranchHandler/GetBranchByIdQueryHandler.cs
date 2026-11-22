using Application.Dtos.Response.Branch;
using Application.Service.Branch.Queries;
using Application.Service.Contracts;
using AutoMapper;
using DomainBranch = Domain.Entities.Branch;
using MediatR;

namespace Infrastructure.Handler.BranchHandler
{
    public class GetBranchByIdQueryHandler : IRequestHandler<GetBranchByIdQuery, BranchResponse?>
    {
        private readonly IGeneralRepositoryInterface<DomainBranch> _branchRepository;
        private readonly IMapper _mapper;

        public GetBranchByIdQueryHandler(
            IGeneralRepositoryInterface<DomainBranch> branchRepository,
            IMapper mapper)
        {
            _branchRepository = branchRepository;
            _mapper = mapper;
        }

        public async Task<BranchResponse?> Handle(
            GetBranchByIdQuery request,
            CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return null;

            var branch = await _branchRepository.GetByIdAsync(request.Id);
            return branch is null ? null : _mapper.Map<BranchResponse>(branch);
        }
    }
}
