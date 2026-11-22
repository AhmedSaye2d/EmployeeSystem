using Application.Service.Contracts;
using Domain.Entities;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using DomainPayroll = Domain.Entities.Payroll;

namespace Infrastructure.Repositories
{
    public class PayrollRepository : GenericRepository<DomainPayroll>, IPayrollRepository
    {
        public PayrollRepository(AppDbContext context) : base(context)
        {
        }

        private IQueryable<DomainPayroll> GetPayrollQuery()
        {
            return _context.Payrolls
                .Include(p => p.Employee)
                    .ThenInclude(e => e.Branch)
                        .ThenInclude(b => b.Department)
                            .ThenInclude(d => d.GeneralDepartment);
        }

        public override async Task<IEnumerable<DomainPayroll>> GetAllAsync()
        {
            return await GetPayrollQuery()
                .AsNoTracking()
                .OrderByDescending(p => p.Year)
                .ThenByDescending(p => p.Month)
                .ThenBy(p => p.EmployeeId)
                .ToListAsync();
        }

        public override async Task<DomainPayroll?> GetByIdAsync(int id)
        {
            return await GetPayrollQuery()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<DomainPayroll?> GetByPeriodAndEmployeeAsync(int employeeId, int month, int year)
        {
            return await GetPayrollQuery()
                .FirstOrDefaultAsync(p => p.EmployeeId == employeeId && p.Month == month && p.Year == year);
        }

        public async Task<IEnumerable<DomainPayroll>> GetByEmployeeIdAsync(int employeeId)
        {
            return await GetPayrollQuery()
                .AsNoTracking()
                .Where(p => p.EmployeeId == employeeId)
                .OrderByDescending(p => p.Year)
                .ThenByDescending(p => p.Month)
                .ToListAsync();
        }

        public async Task<IEnumerable<DomainPayroll>> GetByPeriodAsync(int month, int year)
        {
            return await GetPayrollQuery()
                .AsNoTracking()
                .Where(p => p.Month == month && p.Year == year)
                .OrderBy(p => p.EmployeeId)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int employeeId, int month, int year)
        {
            return await _context.Payrolls
                .AnyAsync(p => p.EmployeeId == employeeId && p.Month == month && p.Year == year);
        }
    }
}
