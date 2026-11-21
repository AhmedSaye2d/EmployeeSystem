using Application.Service.Contracts;
using Domain.Entities;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OverTimeRepository : GenericRepository<OverTime>, IOverTimeRepository
    {
        public OverTimeRepository(AppDbContext context) : base(context)
        {
        }

        private IQueryable<OverTime> GetOverTimeQuery()
        {
            return _context.OverTimes
                .Include(o => o.OverTimeType)
                .Include(o => o.Employee)
                    .ThenInclude(e => e.Branch)
                        .ThenInclude(b => b.Department)
                            .ThenInclude(d => d.GeneralDepartment);
        }

        public override async Task<IEnumerable<OverTime>> GetAllAsync()
        {
            return await GetOverTimeQuery()
                .AsNoTracking()
                .ToListAsync();
        }

        public override async Task<OverTime?> GetByIdAsync(int id)
        {
            return await GetOverTimeQuery()
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<OverTime>> GetByEmployeeIdAsync(int employeeId)
        {
            return await GetOverTimeQuery()
                .AsNoTracking()
                .Where(o => o.EmployeeId == employeeId)
                .ToListAsync();
        }
    }
}