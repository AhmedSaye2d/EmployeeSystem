using MediatR;
using Application.Dtos.Response;

namespace Application.Service.Doctor.Commands
{
    public record DeleteDoctorCommand(int Id) : IRequest<GeneralResponse>;
}
