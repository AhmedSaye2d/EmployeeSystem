using DomainOverTime = Domain.Entities.OverTime;

namespace Application.Service.Contracts
{
    public interface IOverTimeRepository : IGeneralRepositoryInterface<DomainOverTime>
    {
        Task<IEnumerable<DomainOverTime>> GetByEmployeeIdAsync(int employeeId);
    }
}
