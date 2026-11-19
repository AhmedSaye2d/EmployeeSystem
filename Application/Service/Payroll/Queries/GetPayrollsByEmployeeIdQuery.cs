using Application.Dtos.Response.Payroll;
using MediatR;

namespace Application.Service.Payroll.Queries
{
    public record GetPayrollsByEmployeeIdQuery(int EmployeeId) : IRequest<IEnumerable<PayrollResponse>>;
}
