using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.Department.Commands;
using Domain.Entities;
using DomainDepartment = Domain.Entities.Department;
using MediatR;

namespace Infrastructure.Handler.DepartmentHandler
{
    public class UpdateDepartmentHandler : IRequestHandler<UpdateDepartmentCommand, GeneralResponse>
    {
        private readonly IGeneralRepositoryInterface<DomainDepartment> _departmentRepository;
        private readonly IGeneralRepositoryInterface<GeneralDepartment> _generalDepartmentRepository;

        public UpdateDepartmentHandler(
            IGeneralRepositoryInterface<DomainDepartment> departmentRepository,
            IGeneralRepositoryInterface<GeneralDepartment> generalDepartmentRepository)
        {
            _departmentRepository = departmentRepository;
            _generalDepartmentRepository = generalDepartmentRepository;
        }

        public async Task<GeneralResponse> Handle(
            UpdateDepartmentCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Department is null || request.Department.Id <= 0)
                return new GeneralResponse(false, "Invalid Department data or ID.");

            if (string.IsNullOrWhiteSpace(request.Department.Name))
                return new GeneralResponse(false, "Department name is required.");

            if (request.Department.GeneralDepartmentId <= 0)
                return new GeneralResponse(false, "General Department is required.");

            // 1. التحقق من وجود القسم
            var department = await _departmentRepository.GetByIdAsync(request.Department.Id);
            if (department is null)
                return new GeneralResponse(false, "Department not found.");

            // 2. التحقق من وجود الإدارة العامة
            var generalDept = await _generalDepartmentRepository.GetByIdAsync(request.Department.GeneralDepartmentId);
            if (generalDept is null)
                return new GeneralResponse(false, "Selected General Department does not exist.");

            // 3. تحديث البيانات
            department.Name = request.Department.Name.Trim();
            department.GeneralDepartmentId = request.Department.GeneralDepartmentId;

            return await _departmentRepository.UpdateAsync(department);
        }
    }
}
