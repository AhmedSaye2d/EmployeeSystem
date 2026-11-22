using Application.Dtos.Response.VacationType;
using Application.Service.Contracts;
using Application.Service.VacationType.Queries;
using AutoMapper;
using DomainVacationType = Domain.Entities.VacationType;
using MediatR;

namespace Infrastructure.Handler.VacationTypeHandler
{
    public class GetVacationTypeByIdQueryHandler : IRequestHandler<GetVacationTypeByIdQuery, VacationTypeResponse?>
    {
        private readonly IGeneralRepositoryInterface<DomainVacationType> _vacationTypeRepository;
        private readonly IMapper _mapper;

        public GetVacationTypeByIdQueryHandler(
            IGeneralRepositoryInterface<DomainVacationType> vacationTypeRepository,
            IMapper mapper)
        {
            _vacationTypeRepository = vacationTypeRepository;
            _mapper = mapper;
        }

        public async Task<VacationTypeResponse?> Handle(
            GetVacationTypeByIdQuery request,
            CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return null;

            var type = await _vacationTypeRepository.GetByIdAsync(request.Id);
            return type is null ? null : _mapper.Map<VacationTypeResponse>(type);
        }
    }
}
