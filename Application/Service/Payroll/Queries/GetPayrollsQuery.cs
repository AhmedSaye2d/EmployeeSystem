using Application.Dtos.Response.Payroll;
using MediatR;

namespace Application.Service.Payroll.Queries
{
    public record GetPayrollsQuery : IRequest<IEnumerable<PayrollResponse>>;
}
