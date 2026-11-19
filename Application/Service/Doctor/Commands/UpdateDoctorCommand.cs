using MediatR;
using Application.Dtos.Request.Doctor;
using Application.Dtos.Response;

namespace Application.Service.Doctor.Commands
{
    public record UpdateDoctorCommand(UpdateDoctorRequest Doctor) : IRequest<GeneralResponse>;
}
