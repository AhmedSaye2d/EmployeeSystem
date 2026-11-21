using Domain.Entities;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>
    {
        public DepartmentRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _context.Departments
                .AsNoTracking()
                .Include(d => d.GeneralDepartment)
                .ToListAsync();
        }

        public override async Task<Department?> GetByIdAsync(int id)
        {
            return await _context.Departments
                .Include(d => d.GeneralDepartment)
                .FirstOrDefaultAsync(d => d.Id == id);
        }
    }
}
