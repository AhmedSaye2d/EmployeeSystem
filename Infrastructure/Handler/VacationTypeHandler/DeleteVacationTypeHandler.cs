using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.VacationType.Commands;
using DomainVacationType = Domain.Entities.VacationType;
using MediatR;

namespace Infrastructure.Handler.VacationTypeHandler
{
    public class DeleteVacationTypeHandler : IRequestHandler<DeleteVacationTypeCommand, GeneralResponse>
    {
        private readonly IGeneralRepositoryInterface<DomainVacationType> _vacationTypeRepository;

        public DeleteVacationTypeHandler(IGeneralRepositoryInterface<DomainVacationType> vacationTypeRepository)
        {
            _vacationTypeRepository = vacationTypeRepository;
        }

        public async Task<GeneralResponse> Handle(
            DeleteVacationTypeCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return new GeneralResponse(false, "Invalid vacation type ID.");

            return await _vacationTypeRepository.DeleteAsync(request.Id);
        }
    }
}
