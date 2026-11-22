using Application.Dtos.Response.Vacation;
using Application.Service.Contracts;
using Application.Service.Vacation.Queries;
using AutoMapper;
using MediatR;

namespace Infrastructure.Handler.VacationHandler
{
    public class GetVacationByIdQueryHandler : IRequestHandler<GetVacationByIdQuery, VacationResponse?>
    {
        private readonly IVacationRepository _vacationRepository;
        private readonly IMapper _mapper;

        public GetVacationByIdQueryHandler(IVacationRepository vacationRepository, IMapper mapper)
        {
            _vacationRepository = vacationRepository;
            _mapper = mapper;
        }

        public async Task<VacationResponse?> Handle(
            GetVacationByIdQuery request,
            CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return null;

            var vacation = await _vacationRepository.GetByIdAsync(request.Id);
            return vacation is null ? null : _mapper.Map<VacationResponse>(vacation);
        }
    }
}
