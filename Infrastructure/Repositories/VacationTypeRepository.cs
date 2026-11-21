using Domain.Entities;
using Infrastructure.DataAccess;

namespace Infrastructure.Repositories
{
    public class VacationTypeRepository : GenericRepository<VacationType>
    {
        public VacationTypeRepository(AppDbContext context) : base(context)
        {
        }
    }
}
