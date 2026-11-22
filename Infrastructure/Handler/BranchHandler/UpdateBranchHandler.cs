using Application.Dtos.Response;
using Application.Service.Branch.Commands;
using Application.Service.Contracts;
using Domain.Entities;
using DomainBranch = Domain.Entities.Branch;
using MediatR;

namespace Infrastructure.Handler.BranchHandler
{
    public class UpdateBranchHandler : IRequestHandler<UpdateBranchCommand, GeneralResponse>
    {
        private readonly IGeneralRepositoryInterface<DomainBranch> _branchRepository;
        private readonly IGeneralRepositoryInterface<Department> _departmentRepository;

        public UpdateBranchHandler(
            IGeneralRepositoryInterface<DomainBranch> branchRepository,
            IGeneralRepositoryInterface<Department> departmentRepository)
        {
            _branchRepository = branchRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<GeneralResponse> Handle(
            UpdateBranchCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Branch is null || request.Branch.Id <= 0)
                return new GeneralResponse(false, "Invalid branch data or ID.");

            // 1. التحقق من وجود الفرع
            var branch = await _branchRepository.GetByIdAsync(request.Branch.Id);
            if (branch is null)
                return new GeneralResponse(false, "Branch not found.");

            if (request.Branch.GeneralDepartmentId <= 0)
                return new GeneralResponse(false, "General Department is required.");

            if (request.Branch.DepartmentId <= 0)
                return new GeneralResponse(false, "Department is required.");

            // 2. التحقق من وجود القسم
            var department = await _departmentRepository.GetByIdAsync(request.Branch.DepartmentId);
            if (department is null)
                return new GeneralResponse(false, "Selected Department does not exist.");

            // 3. التحقق إن القسم بيتبع الإدارة العامة المحددة
            if (department.GeneralDepartmentId != request.Branch.GeneralDepartmentId)
                return new GeneralResponse(false, "Selected Department does not belong to the selected General Department.");

            // 4. تحديث البيانات
            branch.Name = request.Branch.Name.Trim();
            branch.DepartmentId = request.Branch.DepartmentId;

            return await _branchRepository.UpdateAsync(branch);
        }
    }
}
