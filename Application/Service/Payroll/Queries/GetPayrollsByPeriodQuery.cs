using Application.Dtos.Response.Payroll;
using MediatR;

namespace Application.Service.Payroll.Queries
{
    public record GetPayrollsByPeriodQuery(int Month, int Year) : IRequest<IEnumerable<PayrollResponse>>;
}
