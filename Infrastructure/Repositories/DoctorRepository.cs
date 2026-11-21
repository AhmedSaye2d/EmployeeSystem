using Application.Service.Contracts;
using Domain.Entities;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            return await _context.Doctors
                .AsNoTracking()
                .Include(d => d.Employee)
                    .ThenInclude(e => e.Branch)
                        .ThenInclude(b => b.Department)
                            .ThenInclude(d => d.GeneralDepartment)
                .ToListAsync();
        }

        public override async Task<Doctor?> GetByIdAsync(int id)
        {
            return await _context.Doctors
                .Include(d => d.Employee)
                    .ThenInclude(e => e.Branch)
                        .ThenInclude(b => b.Department)
                            .ThenInclude(d => d.GeneralDepartment)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<Doctor>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _context.Doctors
                .AsNoTracking()
                .Where(d => d.EmployeeId == employeeId)
                .Include(d => d.Employee)
                    .ThenInclude(e => e.Branch)
                        .ThenInclude(b => b.Department)
                            .ThenInclude(d => d.GeneralDepartment)
                .ToListAsync();
        }
    }
}
