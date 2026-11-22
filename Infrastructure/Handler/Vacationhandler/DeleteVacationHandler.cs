using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.Vacation.Commands;
using MediatR;

namespace Infrastructure.Handler.VacationHandler
{
    public class DeleteVacationHandler : IRequestHandler<DeleteVacationCommand, GeneralResponse>
    {
        private readonly IVacationRepository _vacationRepository;

        public DeleteVacationHandler(IVacationRepository vacationRepository)
        {
            _vacationRepository = vacationRepository;
        }

        public async Task<GeneralResponse> Handle(
            DeleteVacationCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return new GeneralResponse(false, "Invalid vacation ID.");

            return await _vacationRepository.DeleteAsync(request.Id);
        }
    }
}
