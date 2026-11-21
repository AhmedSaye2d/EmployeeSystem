using Application.Service.Contracts;
using Domain.Entities;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SanctionRepository : GenericRepository<Sanction>, ISanctionRepository
    {
        public SanctionRepository(AppDbContext context) : base(context)
        {
        }

        private IQueryable<Sanction> GetSanctionQuery()
        {
            return _context.Sanctions
                .Include(s => s.SanctionType)
                .Include(s => s.Employee)
                    .ThenInclude(e => e.Branch)
                        .ThenInclude(b => b.Department)
                            .ThenInclude(d => d.GeneralDepartment);
        }

        public override async Task<IEnumerable<Sanction>> GetAllAsync()
        {
            return await GetSanctionQuery()
                .AsNoTracking()
                .ToListAsync();
        }

        public override async Task<Sanction?> GetByIdAsync(int id)
        {
            return await GetSanctionQuery()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Sanction>> GetByEmployeeIdAsync(int employeeId)
        {
            return await GetSanctionQuery()
                .AsNoTracking()
                .Where(s => s.EmployeeId == employeeId)
                .ToListAsync();
        }
    }
}