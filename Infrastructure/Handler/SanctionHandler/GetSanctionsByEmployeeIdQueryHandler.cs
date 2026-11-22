using Application.Dtos.Response.Sanction;
using Application.Service.Contracts;
using Application.Service.Sanction.Queries;
using AutoMapper;
using MediatR;

namespace Infrastructure.Handler.SanctionHandler
{
    public class GetSanctionsByEmployeeIdQueryHandler : IRequestHandler<GetSanctionsByEmployeeIdQuery, List<SanctionResponse>>
    {
        private readonly ISanctionRepository _sanctionRepository;
        private readonly IMapper _mapper;

        public GetSanctionsByEmployeeIdQueryHandler(
            ISanctionRepository sanctionRepository,
            IMapper mapper)
        {
            _sanctionRepository = sanctionRepository;
            _mapper = mapper;
        }

        public async Task<List<SanctionResponse>> Handle(
            GetSanctionsByEmployeeIdQuery request,
            CancellationToken cancellationToken)
        {
            if (request.EmployeeId <= 0)
                return new List<SanctionResponse>();

            var sanctions = await _sanctionRepository.GetByEmployeeIdAsync(request.EmployeeId);
            return _mapper.Map<List<SanctionResponse>>(sanctions);
        }
    }
}
