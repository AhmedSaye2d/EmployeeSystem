using Application.Dtos.Response;
using Application.Service.Branch.Commands;
using Application.Service.Contracts;
using AutoMapper;
using Domain.Entities;
using DomainBranch = Domain.Entities.Branch;
using MediatR;

namespace Infrastructure.Handler.BranchHandler
{
    public class CreateBranchHandler : IRequestHandler<CreateBranchCommand, GeneralResponse>
    {
        private readonly IGeneralRepositoryInterface<DomainBranch> _branchRepository;
        private readonly IGeneralRepositoryInterface<Department> _departmentRepository;
        private readonly IMapper _mapper;

        public CreateBranchHandler(
            IGeneralRepositoryInterface<DomainBranch> branchRepository,
            IGeneralRepositoryInterface<Department> departmentRepository,
            IMapper mapper)
        {
            _branchRepository = branchRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        public async Task<GeneralResponse> Handle(
            CreateBranchCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Branch is null)
                return new GeneralResponse(false, "Branch data is required.");

            if (string.IsNullOrWhiteSpace(request.Branch.Name))
                return new GeneralResponse(false, "Branch name is required.");

            if (request.Branch.GeneralDepartmentId <= 0)
                return new GeneralResponse(false, "General Department is required.");

            if (request.Branch.DepartmentId <= 0)
                return new GeneralResponse(false, "Department is required.");

            // 1. التحقق من وجود القسم
            var department = await _departmentRepository.GetByIdAsync(request.Branch.DepartmentId);
            if (department is null)
                return new GeneralResponse(false, "Selected Department does not exist.");

            // 2. التحقق إن القسم بيتبع الإدارة العامة المحددة
            if (department.GeneralDepartmentId != request.Branch.GeneralDepartmentId)
                return new GeneralResponse(false, "Selected Department does not belong to the selected General Department.");

            // 3. التحويل والحفظ
            var branch = _mapper.Map<DomainBranch>(request.Branch);
            return await _branchRepository.InsertAsync(branch);
        }
    }
}
