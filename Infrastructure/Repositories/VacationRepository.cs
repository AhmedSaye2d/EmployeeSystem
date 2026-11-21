using Application.Service.Contracts;
using Domain.Entities;
using Domain.Entities.Enum;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class VacationRepository : GenericRepository<Vacation>, IVacationRepository
    {
        public VacationRepository(AppDbContext context) : base(context)
        {
        }

        private IQueryable<Vacation> GetVacationQuery()
        {
            return _context.Vacations
                .AsNoTracking()
                .Include(v => v.VacationType)
                .Include(v => v.Employee)
                    .ThenInclude(e => e.Branch)
                        .ThenInclude(b => b.Department)
                            .ThenInclude(d => d.GeneralDepartment);
        }

        public override async Task<IEnumerable<Vacation>> GetAllAsync()
        {
            return await GetVacationQuery().ToListAsync();
        }

        public override async Task<Vacation?> GetByIdAsync(int id)
        {
            return await GetVacationQuery()
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<Vacation>> GetByEmployeeIdAsync(int employeeId)
        {
            return await GetVacationQuery()
                .Where(v => v.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Vacation>> GetPendingVacationsAsync()
        {
            return await GetVacationQuery()
                .Where(v => v.Status == VacationStatus.Pending)
                .ToListAsync();
        }
    }
}
