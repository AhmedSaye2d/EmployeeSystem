using Application.Dtos.Response.OverTimeType;
using Application.Service.Contracts;
using Application.Service.OverTimeType.Queries;
using AutoMapper;
using DomainOverTimeType = Domain.Entities.OverTimeType;
using MediatR;

namespace Infrastructure.Handler.OverTimeTypeHandler
{
    public class GetOverTimeTypeByIdQueryHandler : IRequestHandler<GetOverTimeTypeByIdQuery, OverTimeTypeResponse?>
    {
        private readonly IGeneralRepositoryInterface<DomainOverTimeType> _overTimeTypeRepository;
        private readonly IMapper _mapper;

        public GetOverTimeTypeByIdQueryHandler(
            IGeneralRepositoryInterface<DomainOverTimeType> overTimeTypeRepository,
            IMapper mapper)
        {
            _overTimeTypeRepository = overTimeTypeRepository;
            _mapper = mapper;
        }

        public async Task<OverTimeTypeResponse?> Handle(
            GetOverTimeTypeByIdQuery request,
            CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return null;

            var type = await _overTimeTypeRepository.GetByIdAsync(request.Id);
            return type is null ? null : _mapper.Map<OverTimeTypeResponse>(type);
        }
    }
}
