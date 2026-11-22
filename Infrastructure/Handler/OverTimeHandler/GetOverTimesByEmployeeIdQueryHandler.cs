using Application.Dtos.Response.OverTime;
using Application.Service.Contracts;
using Application.Service.OverTime.Queries;
using AutoMapper;
using MediatR;

namespace Infrastructure.Handler.OverTimeHandler
{
    public class GetOverTimesByEmployeeIdQueryHandler : IRequestHandler<GetOverTimesByEmployeeIdQuery, List<OverTimeResponse>>
    {
        private readonly IOverTimeRepository _overTimeRepository;
        private readonly IMapper _mapper;

        public GetOverTimesByEmployeeIdQueryHandler(
            IOverTimeRepository overTimeRepository,
            IMapper mapper)
        {
            _overTimeRepository = overTimeRepository;
            _mapper = mapper;
        }

        public async Task<List<OverTimeResponse>> Handle(
            GetOverTimesByEmployeeIdQuery request,
            CancellationToken cancellationToken)
        {
            if (request.EmployeeId <= 0)
                return new List<OverTimeResponse>();

            var overtimes = await _overTimeRepository.GetByEmployeeIdAsync(request.EmployeeId);
            return _mapper.Map<List<OverTimeResponse>>(overtimes);
        }
    }
}
