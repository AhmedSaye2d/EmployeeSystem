using Domain.Entities;
using Infrastructure.DataAccess;

namespace Infrastructure.Repositories
{
    public class OverTimeTypeRepository : GenericRepository<OverTimeType>
    {
        public OverTimeTypeRepository(AppDbContext context) : base(context)
        {
        }
    }
}
