using DomainSanction = Domain.Entities.Sanction;

namespace Application.Service.Contracts
{
    public interface ISanctionRepository : IGeneralRepositoryInterface<DomainSanction>
    {
        Task<IEnumerable<DomainSanction>> GetByEmployeeIdAsync(int employeeId);
    }
}
