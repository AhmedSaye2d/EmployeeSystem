using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.VacationType.Commands;
using AutoMapper;
using DomainVacationType = Domain.Entities.VacationType;
using MediatR;

namespace Infrastructure.Handler.VacationTypeHandler
{
    public class CreateVacationTypeHandler : IRequestHandler<CreateVacationTypeCommand, GeneralResponse>
    {
        private readonly IGeneralRepositoryInterface<DomainVacationType> _vacationTypeRepository;
        private readonly IMapper _mapper;

        public CreateVacationTypeHandler(
            IGeneralRepositoryInterface<DomainVacationType> vacationTypeRepository,
            IMapper mapper)
        {
            _vacationTypeRepository = vacationTypeRepository;
            _mapper = mapper;
        }

        public async Task<GeneralResponse> Handle(
            CreateVacationTypeCommand request,
            CancellationToken cancellationToken)
        {
            if (request.VacationType is null)
                return new GeneralResponse(false, "Vacation type data is required.");

            if (string.IsNullOrWhiteSpace(request.VacationType.Name))
                return new GeneralResponse(false, "Vacation type name is required.");

            var vacationType = _mapper.Map<DomainVacationType>(request.VacationType);
            return await _vacationTypeRepository.InsertAsync(vacationType);
        }
    }
}
