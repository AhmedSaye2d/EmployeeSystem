using Application.Dtos.Response;
using DomainEmployee = Domain.Entities.Employee;

namespace Application.Service.Contracts
{
    public interface IEmployeeRepository : IGeneralRepositoryInterface<DomainEmployee>
    {
        Task<DomainEmployee?> GetByCivilIdAsync(string civilId);
        Task<DomainEmployee?> GetByFileNumberAsync(string fileNumber);
        Task<IEnumerable<DomainEmployee>> SearchAsync(string searchTerm);
    }
}
