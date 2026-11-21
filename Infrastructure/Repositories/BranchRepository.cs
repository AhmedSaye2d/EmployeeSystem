using Domain.Entities;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BranchRepository : GenericRepository<Branch>
    {
        public BranchRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Branch>> GetAllAsync()
        {
            return await _context.Branches
                .AsNoTracking()
                .Include(b => b.Department)
                    .ThenInclude(d => d.GeneralDepartment)
                .ToListAsync();
        }

        public override async Task<Branch?> GetByIdAsync(int id)
        {
            return await _context.Branches
                .Include(b => b.Department)
                    .ThenInclude(d => d.GeneralDepartment)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}
