using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.Department.Commands;
using DomainDepartment = Domain.Entities.Department;
using MediatR;

namespace Infrastructure.Handler.DepartmentHandler
{
    public class DeleteDepartmentHandler : IRequestHandler<DeleteDepartmentCommand, GeneralResponse>
    {
        private readonly IGeneralRepositoryInterface<DomainDepartment> _departmentRepository;

        public DeleteDepartmentHandler(IGeneralRepositoryInterface<DomainDepartment> departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<GeneralResponse> Handle(
            DeleteDepartmentCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return new GeneralResponse(false, "Invalid Department ID.");

            return await _departmentRepository.DeleteAsync(request.Id);
        }
    }
}
