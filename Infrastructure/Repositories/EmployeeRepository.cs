using Application.Dtos.Response;
using Application.Service.Contracts;
using Domain.Entities;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees
                .AsNoTracking()
                .Include(e => e.Branch)
                    .ThenInclude(b => b.Department)
                        .ThenInclude(d => d.GeneralDepartment)
                .Include(e => e.Town)
                    .ThenInclude(t => t.City)
                        .ThenInclude(c => c.Country)
                .ToListAsync();
        }

        public override async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.Branch)
                    .ThenInclude(b => b.Department)
                        .ThenInclude(d => d.GeneralDepartment)

                .Include(e => e.Town)
                    .ThenInclude(t => t.City)
                        .ThenInclude(c => c.Country)

                .Include(e => e.Vacations)
                    .ThenInclude(v => v.VacationType)

                .Include(e => e.OverTimes)
                    .ThenInclude(o => o.OverTimeType)

                .Include(e => e.Doctors)

                .Include(e => e.Sanctions)
                    .ThenInclude(s => s.SanctionType)

                .AsSplitQuery()

                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Employee?> GetByCivilIdAsync(string civilId)
        {
            return await _context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.CivilId == civilId);
        }

        public async Task<Employee?> GetByFileNumberAsync(string fileNumber)
        {
            return await _context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.FileNumber == fileNumber);
        }

        public async Task<IEnumerable<Employee>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            var term = searchTerm.Trim().ToLower();

            return await _context.Employees
                .AsNoTracking()
                .Include(e => e.Branch)
                    .ThenInclude(b => b.Department)
                        .ThenInclude(d => d.GeneralDepartment)
                .Include(e => e.Town)
                    .ThenInclude(t => t.City)
                        .ThenInclude(c => c.Country)
                .Where(e => (e.Name != null && e.Name.ToLower().Contains(term))
                         || (e.Fullname != null && e.Fullname.ToLower().Contains(term))
                         || (e.CivilId != null && e.CivilId.ToLower().Contains(term))
                         || (e.FileNumber != null && e.FileNumber.ToLower().Contains(term))
                         || (e.JobName != null && e.JobName.ToLower().Contains(term)))
                .ToListAsync();
        }
    }
}
