using Application.Dtos.Response.Sanction;
using Application.Service.Contracts;
using Application.Service.Sanction.Queries;
using AutoMapper;
using Domain.Entities;
using DomainSanction = Domain.Entities.Sanction;
using MediatR;

namespace Infrastructure.Handler.SanctionHandler
{
    public class GetSanctionsQueryHandler : IRequestHandler<GetSanctionsQuery, List<SanctionResponse>>
    {
        private readonly IGeneralRepositoryInterface<DomainSanction> _sanctionRepository;
        private readonly IMapper _mapper;

        public GetSanctionsQueryHandler(
            IGeneralRepositoryInterface<DomainSanction> sanctionRepository,
            IMapper mapper)
        {
            _sanctionRepository = sanctionRepository;
            _mapper = mapper;
        }

        public async Task<List<SanctionResponse>> Handle(
            GetSanctionsQuery request,
            CancellationToken cancellationToken)
        {
            var sanctions = await _sanctionRepository.GetAllAsync();
            return _mapper.Map<List<SanctionResponse>>(sanctions);
        }
    }
}
