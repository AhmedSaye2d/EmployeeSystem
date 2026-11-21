using Domain.Entities;
using Infrastructure.DataAccess;

namespace Infrastructure.Repositories
{
    public class SanctionTypeRepository : GenericRepository<SanctionType>
    {
        public SanctionTypeRepository(AppDbContext context) : base(context)
        {
        }
    }
}
