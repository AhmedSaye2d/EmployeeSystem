using Application.Dtos.Response.OverTime;
using Application.Service.Contracts;
using Application.Service.OverTime.Queries;
using AutoMapper;
using MediatR;

namespace Infrastructure.Handler.OverTimeHandler
{
    public class GetOverTimesQueryHandler : IRequestHandler<GetOverTimesQuery, List<OverTimeResponse>>
    {
        private readonly IOverTimeRepository _overTimeRepository;
        private readonly IMapper _mapper;

        public GetOverTimesQueryHandler(
            IOverTimeRepository overTimeRepository,
            IMapper mapper)
        {
            _overTimeRepository = overTimeRepository;
            _mapper = mapper;
        }

        public async Task<List<OverTimeResponse>> Handle(
            GetOverTimesQuery request,
            CancellationToken cancellationToken)
        {
            var overtimes = await _overTimeRepository.GetAllAsync();
            return _mapper.Map<List<OverTimeResponse>>(overtimes);
        }
    }
}
