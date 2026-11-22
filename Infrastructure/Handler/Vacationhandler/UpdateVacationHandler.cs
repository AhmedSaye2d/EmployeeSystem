using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.Vacation.Commands;
using Domain.Entities;
using MediatR;

namespace Infrastructure.Handler.VacationHandler
{
    public class UpdateVacationHandler : IRequestHandler<UpdateVacationCommand, GeneralResponse>
    {
        private readonly IVacationRepository _vacationRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IGeneralRepositoryInterface<VacationType> _vacationTypeRepository;

        public UpdateVacationHandler(
            IVacationRepository vacationRepository,
            IEmployeeRepository employeeRepository,
            IGeneralRepositoryInterface<VacationType> vacationTypeRepository)
        {
            _vacationRepository = vacationRepository;
            _employeeRepository = employeeRepository;
            _vacationTypeRepository = vacationTypeRepository;
        }

        public async Task<GeneralResponse> Handle(
            UpdateVacationCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Vacation is null || request.Vacation.Id <= 0)
                return new GeneralResponse(false, "Invalid vacation data or ID.");

            // 1. التحقق من وجود الإجازة
            var vacation = await _vacationRepository.GetByIdAsync(request.Vacation.Id);
            if (vacation is null)
                return new GeneralResponse(false, "Vacation not found.");

            // 2. التحقق من وجود الموظف
            var employee = await _employeeRepository.GetByIdAsync(request.Vacation.EmployeeId);
            if (employee is null)
                return new GeneralResponse(false, "Employee not found.");

            // 3. التحقق من وجود نوع الإجازة
            var vacationType = await _vacationTypeRepository.GetByIdAsync(request.Vacation.VacationTypeId);
            if (vacationType is null)
                return new GeneralResponse(false, "Selected Vacation Type does not exist.");

            // 4. التحقق من عدد الأيام
            if (request.Vacation.NumberOfDays <= 0)
                return new GeneralResponse(false, "Number of days must be at least 1.");

            // 5. تحديث البيانات
            vacation.EmployeeId = request.Vacation.EmployeeId;
            vacation.VacationTypeId = request.Vacation.VacationTypeId;
            vacation.StartData = request.Vacation.StartData;
            vacation.NumberOfDays = request.Vacation.NumberOfDays;
            vacation.Other = request.Vacation.Other;

            return await _vacationRepository.UpdateAsync(vacation);
        }
    }
}
