using Application.Dtos.Response.Vacation;
using Application.Service.Contracts;
using Application.Service.Vacation.Queries;
using AutoMapper;
using MediatR;

namespace Infrastructure.Handler.VacationHandler
{
    public class GetVacationsByEmployeeIdQueryHandler : IRequestHandler<GetVacationsByEmployeeIdQuery, List<VacationResponse>>
    {
        private readonly IVacationRepository _vacationRepository;
        private readonly IMapper _mapper;

        public GetVacationsByEmployeeIdQueryHandler(IVacationRepository vacationRepository, IMapper mapper)
        {
            _vacationRepository = vacationRepository;
            _mapper = mapper;
        }

        public async Task<List<VacationResponse>> Handle(
            GetVacationsByEmployeeIdQuery request,
            CancellationToken cancellationToken)
        {
            if (request.EmployeeId <= 0)
                return new List<VacationResponse>();

            var vacations = await _vacationRepository.GetByEmployeeIdAsync(request.EmployeeId);
            return _mapper.Map<List<VacationResponse>>(vacations);
        }
    }
}
