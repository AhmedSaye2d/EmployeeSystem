using DomainDoctor = Domain.Entities.Doctor;

namespace Application.Service.Contracts
{
    public interface IDoctorRepository : IGeneralRepositoryInterface<DomainDoctor>
    {
        Task<IEnumerable<DomainDoctor>> GetByEmployeeIdAsync(int employeeId);
    }
}
