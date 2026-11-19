using MediatR;
using Application.Dtos.Response.VacationType;

namespace Application.Service.VacationType.Queries
{
    public record GetVacationTypesQuery : IRequest<List<VacationTypeResponse>>;
}
