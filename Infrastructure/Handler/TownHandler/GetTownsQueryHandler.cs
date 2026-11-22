using Application.Dtos.Response.Town;
using Application.Service.Contracts;
using Application.Service.Town.Queries;
using AutoMapper;
using DomainTown = Domain.Entities.Town;
using MediatR;

namespace Infrastructure.Handler.TownHandler
{
    public class GetTownsQueryHandler : IRequestHandler<GetTownsQuery, List<TownResponse>>
    {
        private readonly IGeneralRepositoryInterface<DomainTown> _townRepository;
        private readonly IMapper _mapper;

        public GetTownsQueryHandler(
            IGeneralRepositoryInterface<DomainTown> townRepository,
            IMapper mapper)
        {
            _townRepository = townRepository;
            _mapper = mapper;
        }

        public async Task<List<TownResponse>> Handle(
            GetTownsQuery request,
            CancellationToken cancellationToken)
        {
            var towns = await _townRepository.GetAllAsync();
            return _mapper.Map<List<TownResponse>>(towns);
        }
    }
}
