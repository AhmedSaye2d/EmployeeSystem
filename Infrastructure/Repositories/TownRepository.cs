using Domain.Entities;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TownRepository : GenericRepository<Town>
    {
        public TownRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Town>> GetAllAsync()
        {
            return await _context.Towns
                .AsNoTracking()
                .Include(t => t.City)
                    .ThenInclude(c => c.Country)
                .ToListAsync();
        }

        public override async Task<Town?> GetByIdAsync(int id)
        {
            return await _context.Towns
                .Include(t => t.City)
                    .ThenInclude(c => c.Country)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
