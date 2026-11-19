using Application.Dtos.Response.Doctor;
using MediatR;

namespace Application.Service.Doctor.Queries
{
    public record GetDoctorsByEmployeeIdQuery(int EmployeeId) : IRequest<List<DoctorResponse>>;
}
