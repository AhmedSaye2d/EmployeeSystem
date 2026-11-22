using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.Doctor.Commands;
using MediatR;

namespace Infrastructure.Handler.DoctorHandler
{
    public class DeleteDoctorHandler : IRequestHandler<DeleteDoctorCommand, GeneralResponse>
    {
        private readonly IDoctorRepository _doctorRepository;

        public DeleteDoctorHandler(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<GeneralResponse> Handle(
            DeleteDoctorCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return new GeneralResponse(false, "Invalid doctor ID.");

            return await _doctorRepository.DeleteAsync(request.Id);
        }
    }
}
