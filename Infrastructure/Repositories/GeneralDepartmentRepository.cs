using Domain.Entities;
using Infrastructure.DataAccess;

namespace Infrastructure.Repositories
{
    public class GeneralDepartmentRepository : GenericRepository<GeneralDepartment>
    {
        public GeneralDepartmentRepository(AppDbContext context) : base(context)
        {

        }
    }
}
