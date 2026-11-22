using Application.Dtos.Response.OverTimeType;
using Application.Service.Contracts;
using Application.Service.OverTimeType.Queries;
using AutoMapper;
using DomainOverTimeType = Domain.Entities.OverTimeType;
using MediatR;

namespace Infrastructure.Handler.OverTimeTypeHandler
{
    public class GetOverTimeTypesQueryHandler : IRequestHandler<GetOverTimeTypesQuery, List<OverTimeTypeResponse>>
    {
        private readonly IGeneralRepositoryInterface<DomainOverTimeType> _overTimeTypeRepository;
        private readonly IMapper _mapper;

        public GetOverTimeTypesQueryHandler(
            IGeneralRepositoryInterface<DomainOverTimeType> overTimeTypeRepository,
            IMapper mapper)
        {
            _overTimeTypeRepository = overTimeTypeRepository;
            _mapper = mapper;
        }

        public async Task<List<OverTimeTypeResponse>> Handle(
            GetOverTimeTypesQuery request,
            CancellationToken cancellationToken)
        {
            var types = await _overTimeTypeRepository.GetAllAsync();
            return _mapper.Map<List<OverTimeTypeResponse>>(types);
        }
    }
}
