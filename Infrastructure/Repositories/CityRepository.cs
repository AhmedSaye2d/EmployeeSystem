using Domain.Entities;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CityRepository : GenericRepository<City>
    {
        public CityRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<City>> GetAllAsync()
        {
            return await _context.Cities
                .AsNoTracking()
                .Include(c => c.Country)
                .ToListAsync();
        }

        public override async Task<City?> GetByIdAsync(int id)
        {
            return await _context.Cities
                .Include(c => c.Country)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
