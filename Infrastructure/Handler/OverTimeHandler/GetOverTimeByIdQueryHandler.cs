using Application.Dtos.Response.OverTime;
using Application.Service.Contracts;
using Application.Service.OverTime.Queries;
using AutoMapper;
using MediatR;

namespace Infrastructure.Handler.OverTimeHandler
{
    public class GetOverTimeByIdQueryHandler : IRequestHandler<GetOverTimeByIdQuery, OverTimeResponse?>
    {
        private readonly IOverTimeRepository _overTimeRepository;
        private readonly IMapper _mapper;

        public GetOverTimeByIdQueryHandler(
            IOverTimeRepository overTimeRepository,
            IMapper mapper)
        {
            _overTimeRepository = overTimeRepository;
            _mapper = mapper;
        }

        public async Task<OverTimeResponse?> Handle(
            GetOverTimeByIdQuery request,
            CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return null;

            var overTime = await _overTimeRepository.GetByIdAsync(request.Id);
            return overTime is null ? null : _mapper.Map<OverTimeResponse>(overTime);
        }
    }
}
