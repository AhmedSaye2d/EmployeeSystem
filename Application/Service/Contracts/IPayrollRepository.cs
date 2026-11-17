using DomainPayroll = Domain.Entities.Payroll;

namespace Application.Service.Contracts
{
    public interface IPayrollRepository : IGeneralRepositoryInterface<DomainPayroll>
    {
        Task<DomainPayroll?> GetByPeriodAndEmployeeAsync(int employeeId, int month, int year);
        Task<IEnumerable<DomainPayroll>> GetByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<DomainPayroll>> GetByPeriodAsync(int month, int year);
        Task<bool> ExistsAsync(int employeeId, int month, int year);
    }
}
