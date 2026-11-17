using DomainVacation = Domain.Entities.Vacation;

namespace Application.Service.Contracts
{
    public interface IVacationRepository : IGeneralRepositoryInterface<DomainVacation>
    {
        Task<IEnumerable<DomainVacation>> GetByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<DomainVacation>> GetPendingVacationsAsync();
    }
}
