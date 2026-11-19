using MediatR;
using Application.Dtos.Response.Doctor;

namespace Application.Service.Doctor.Queries
{
    public record GetDoctorsQuery : IRequest<List<DoctorResponse>>;
}
