using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.Employee.Commands;
using Domain.Entities;
using Mapster;
using MediatR;

namespace Infrastructure.Handler.EmployeeHandler
{
    public class CreateEmployeeHandler : IRequestHandler<CreateEmployeeCommand, GeneralResponse>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IGeneralRepositoryInterface<Branch> _branchRepository;
        private readonly IGeneralRepositoryInterface<Town> _townRepository;

        public CreateEmployeeHandler(
            IEmployeeRepository employeeRepository,
            IGeneralRepositoryInterface<Branch> branchRepository,
            IGeneralRepositoryInterface<Town> townRepository)
        {
            _employeeRepository = employeeRepository;
            _branchRepository = branchRepository;
            _townRepository = townRepository;
        }

        public async Task<GeneralResponse> Handle(
            CreateEmployeeCommand request,
            CancellationToken cancellationToken)
        {
            // 1. فحص وجود البيانات
            if (request.Employee is null)
                return new GeneralResponse(false, "Employee data is required.");

            // 2. فحص عدم تكرار الرقم المدني
            var civilExists = await _employeeRepository.GetByCivilIdAsync(request.Employee.CivilId);
            if (civilExists is not null)
                return new GeneralResponse(false, "Civil ID already exists.");

            // 3. فحص عدم تكرار رقم الملف
            var fileExists = await _employeeRepository.GetByFileNumberAsync(request.Employee.FileNumber);
            if (fileExists is not null)
                return new GeneralResponse(false, "File Number already exists.");

            // 4. فحص وجود الفرع
            var branch = await _branchRepository.GetByIdAsync(request.Employee.BranchId);
            if (branch is null)
                return new GeneralResponse(false, "Selected Branch does not exist.");

            // 5. فحص وجود البلدة/المدينة
            var town = await _townRepository.GetByIdAsync(request.Employee.TownId);
            if (town is null)
                return new GeneralResponse(false, "Selected Town does not exist.");

            // 6. تحويل DTO إلى Entity وحفظها في قاعدة البيانات
            var employee = request.Employee.Adapt<Employee>();
            return await _employeeRepository.InsertAsync(employee);
        }
    }
}
