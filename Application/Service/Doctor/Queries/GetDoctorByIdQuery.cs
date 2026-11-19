using MediatR;
using Application.Dtos.Response.Doctor;

namespace Application.Service.Doctor.Queries
{
    public record GetDoctorByIdQuery(int Id) : IRequest<DoctorResponse?>;
}
