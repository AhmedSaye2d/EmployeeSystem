using Application.Dtos.Response.Sanction;
using Application.Service.Contracts;
using Application.Service.Sanction.Queries;
using AutoMapper;
using Domain.Entities;
using DomainSanction = Domain.Entities.Sanction;
using MediatR;

namespace Infrastructure.Handler.SanctionHandler
{
    public class GetSanctionByIdQueryHandler : IRequestHandler<GetSanctionByIdQuery, SanctionResponse?>
    {
        private readonly IGeneralRepositoryInterface<DomainSanction> _sanctionRepository;
        private readonly IMapper _mapper;

        public GetSanctionByIdQueryHandler(
            IGeneralRepositoryInterface<DomainSanction> sanctionRepository,
            IMapper mapper)
        {
            _sanctionRepository = sanctionRepository;
            _mapper = mapper;
        }

        public async Task<SanctionResponse?> Handle(
            GetSanctionByIdQuery request,
            CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return null;

            var sanction = await _sanctionRepository.GetByIdAsync(request.Id);
            return sanction is null ? null : _mapper.Map<SanctionResponse>(sanction);
        }
    }
}
