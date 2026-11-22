using Application.Dtos.Response.Vacation;
using Application.Service.Contracts;
using Application.Service.Vacation.Queries;
using AutoMapper;
using MediatR;

namespace Infrastructure.Handler.VacationHandler
{
    public class GetPendingVacationsQueryHandler : IRequestHandler<GetPendingVacationsQuery, IEnumerable<VacationResponse>>
    {
        private readonly IVacationRepository _vacationRepository;
        private readonly IMapper _mapper;

        public GetPendingVacationsQueryHandler(IVacationRepository vacationRepository, IMapper mapper)
        {
            _vacationRepository = vacationRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VacationResponse>> Handle(
            GetPendingVacationsQuery request,
            CancellationToken cancellationToken)
        {
            var vacations = await _vacationRepository.GetPendingVacationsAsync();
            return _mapper.Map<IEnumerable<VacationResponse>>(vacations);
        }
    }
}
