using Application.Dtos.Response.Payroll;
using MediatR;

namespace Application.Service.Payroll.Queries
{
    public record GetPayrollByIdQuery(int Id) : IRequest<PayrollResponse?>;
}
