using Domain.Entities;
using Infrastructure.DataAccess;

namespace Infrastructure.Repositories
{
    public class CountryRepository : GenericRepository<Country>
    {
        public CountryRepository(AppDbContext context) : base(context)
        {
        }
    }
}
