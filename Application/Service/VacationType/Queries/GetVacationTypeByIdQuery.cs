using MediatR;
using Application.Dtos.Response.VacationType;

namespace Application.Service.VacationType.Queries
{
    public record GetVacationTypeByIdQuery(int Id) : IRequest<VacationTypeResponse?>;
}
