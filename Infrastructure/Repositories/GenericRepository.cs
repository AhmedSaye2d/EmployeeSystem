using Application.Dtos.Response;
using Application.Service.Contracts;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IGeneralRepositoryInterface<T> where T : class
    {
        protected readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<GeneralResponse> InsertAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return new GeneralResponse(true, "Added successfully");
        }

        public virtual async Task<GeneralResponse> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            return new GeneralResponse(true, "Updated successfully");
        }

        public virtual async Task<GeneralResponse> DeleteAsync(int id)
        {
            var item = await _context.Set<T>().FindAsync(id);
            if (item is null)
                return new GeneralResponse(false, "Item not found");

            _context.Set<T>().Remove(item);
            await _context.SaveChangesAsync();
            return new GeneralResponse(true, "Deleted successfully");
        }
    }
}
