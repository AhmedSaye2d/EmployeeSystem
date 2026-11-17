using Application.Dtos.Response;

namespace Application.Service.Contracts
{
    public interface IGeneralRepositoryInterface<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<GeneralResponse> InsertAsync(T entity);
        Task<GeneralResponse> UpdateAsync(T entity);
        Task<GeneralResponse> DeleteAsync(int id);
    }
}
