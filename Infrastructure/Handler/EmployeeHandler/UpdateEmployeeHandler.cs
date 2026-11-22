using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.Employee.Commands;
using Domain.Entities;
using Mapster;
using MediatR;

namespace Infrastructure.Handler.EmployeeHandler
{
    public class UpdateEmployeeHandler : IRequestHandler<UpdateEmployeeCommand, GeneralResponse>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IGeneralRepositoryInterface<Branch> _branchRepository;
        private readonly IGeneralRepositoryInterface<Town> _townRepository;

        public UpdateEmployeeHandler(
            IEmployeeRepository employeeRepository,
            IGeneralRepositoryInterface<Branch> branchRepository,
            IGeneralRepositoryInterface<Town> townRepository)
        {
            _employeeRepository = employeeRepository;
            _branchRepository = branchRepository;
            _townRepository = townRepository;
        }

        public async Task<GeneralResponse> Handle(
            UpdateEmployeeCommand request,
            CancellationToken cancellationToken)
        {
            // 1. فحص صحة البيانات والـ ID
            if (request.Employee is null || request.Employee.Id <= 0)
                return new GeneralResponse(false, "Invalid employee data or ID.");

            // 2. فحص وجود الموظف
            var employee = await _employeeRepository.GetByIdAsync(request.Employee.Id);
            if (employee is null)
                return new GeneralResponse(false, "Employee not found.");

            // 3. فحص عدم تكرار الرقم المدني مع موظف آخر
            var civilEmployee = await _employeeRepository.GetByCivilIdAsync(request.Employee.CivilId);
            if (civilEmployee is not null && civilEmployee.Id != request.Employee.Id)
                return new GeneralResponse(false, "Civil ID is already used by another employee.");

            // 4. فحص عدم تكرار رقم الملف مع موظف آخر
            var fileEmployee = await _employeeRepository.GetByFileNumberAsync(request.Employee.FileNumber);
            if (fileEmployee is not null && fileEmployee.Id != request.Employee.Id)
                return new GeneralResponse(false, "File Number is already used by another employee.");

            // 5. فحص وجود الفرع
            var branch = await _branchRepository.GetByIdAsync(request.Employee.BranchId);
            if (branch is null)
                return new GeneralResponse(false, "Selected Branch does not exist.");

            // 6. فحص وجود البلدة/المدينة
            var town = await _townRepository.GetByIdAsync(request.Employee.TownId);
            if (town is null)
                return new GeneralResponse(false, "Selected Town does not exist.");

            // 7. تحديث الخصائص بـ Mapster وحفظها
            request.Employee.Adapt(employee);
            return await _employeeRepository.UpdateAsync(employee);
        }
    }
}
