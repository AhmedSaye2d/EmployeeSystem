using Application.Dtos.Response.Town;
using Application.Service.Contracts;
using Application.Service.Town.Queries;
using AutoMapper;
using DomainTown = Domain.Entities.Town;
using MediatR;

namespace Infrastructure.Handler.TownHandler
{
    public class GetTownByIdQueryHandler : IRequestHandler<GetTownByIdQuery, TownResponse?>
    {
        private readonly IGeneralRepositoryInterface<DomainTown> _townRepository;
        private readonly IMapper _mapper;

        public GetTownByIdQueryHandler(
            IGeneralRepositoryInterface<DomainTown> townRepository,
            IMapper mapper)
        {
            _townRepository = townRepository;
            _mapper = mapper;
        }

        public async Task<TownResponse?> Handle(
            GetTownByIdQuery request,
            CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return null;

            var town = await _townRepository.GetByIdAsync(request.Id);
            return town is null ? null : _mapper.Map<TownResponse>(town);
        }
    }
}
