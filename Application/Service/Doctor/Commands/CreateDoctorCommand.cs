using MediatR;
using Application.Dtos.Request.Doctor;
using Application.Dtos.Response;

namespace Application.Service.Doctor.Commands
{
    public record CreateDoctorCommand(AddDoctorRequest Doctor) : IRequest<GeneralResponse>;
}
