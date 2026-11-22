using Application.Dtos.Response.Branch;
using Application.Service.Branch.Queries;
using Application.Service.Contracts;
using AutoMapper;
using DomainBranch = Domain.Entities.Branch;
using MediatR;

namespace Infrastructure.Handler.BranchHandler
{
    public class GetBranchesQueryHandler : IRequestHandler<GetBranchesQuery, List<BranchResponse>>
    {
        private readonly IGeneralRepositoryInterface<DomainBranch> _branchRepository;
        private readonly IMapper _mapper;

        public GetBranchesQueryHandler(
            IGeneralRepositoryInterface<DomainBranch> branchRepository,
            IMapper mapper)
        {
            _branchRepository = branchRepository;
            _mapper = mapper;
        }

        public async Task<List<BranchResponse>> Handle(
            GetBranchesQuery request,
            CancellationToken cancellationToken)
        {
            var branches = await _branchRepository.GetAllAsync();
            return _mapper.Map<List<BranchResponse>>(branches);
        }
    }
}
