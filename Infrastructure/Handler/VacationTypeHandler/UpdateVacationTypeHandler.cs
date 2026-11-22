using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.VacationType.Commands;
using DomainVacationType = Domain.Entities.VacationType;
using MediatR;

namespace Infrastructure.Handler.VacationTypeHandler
{
    public class UpdateVacationTypeHandler : IRequestHandler<UpdateVacationTypeCommand, GeneralResponse>
    {
        private readonly IGeneralRepositoryInterface<DomainVacationType> _vacationTypeRepository;

        public UpdateVacationTypeHandler(IGeneralRepositoryInterface<DomainVacationType> vacationTypeRepository)
        {
            _vacationTypeRepository = vacationTypeRepository;
        }

        public async Task<GeneralResponse> Handle(
            UpdateVacationTypeCommand request,
            CancellationToken cancellationToken)
        {
            if (request.VacationType is null || request.VacationType.Id <= 0)
                return new GeneralResponse(false, "Invalid vacation type data or ID.");

            if (string.IsNullOrWhiteSpace(request.VacationType.Name))
                return new GeneralResponse(false, "Vacation type name is required.");

            var vacationType = await _vacationTypeRepository.GetByIdAsync(request.VacationType.Id);
            if (vacationType is null)
                return new GeneralResponse(false, "Vacation type not found.");

            vacationType.Name = request.VacationType.Name.Trim();
            return await _vacationTypeRepository.UpdateAsync(vacationType);
        }
    }
}
