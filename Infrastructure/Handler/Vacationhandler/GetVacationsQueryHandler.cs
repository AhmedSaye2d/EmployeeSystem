using Application.Dtos.Response.Vacation;
using Application.Service.Contracts;
using Application.Service.Vacation.Queries;
using AutoMapper;
using MediatR;

namespace Infrastructure.Handler.VacationHandler
{
    public class GetVacationsQueryHandler : IRequestHandler<GetVacationsQuery, List<VacationResponse>>
    {
        private readonly IVacationRepository _vacationRepository;
        private readonly IMapper _mapper;

        public GetVacationsQueryHandler(IVacationRepository vacationRepository, IMapper mapper)
        {
            _vacationRepository = vacationRepository;
            _mapper = mapper;
        }

        public async Task<List<VacationResponse>> Handle(
            GetVacationsQuery request,
            CancellationToken cancellationToken)
        {
            var vacations = await _vacationRepository.GetAllAsync();
            return _mapper.Map<List<VacationResponse>>(vacations);
        }
    }
}
