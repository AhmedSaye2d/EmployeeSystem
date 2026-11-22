using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.Vacation.Commands;
using AutoMapper;
using DomainVacation = Domain.Entities.Vacation;
using Domain.Entities;
using MediatR;
using Domain.Entities.Enum;

namespace Infrastructure.Handler.VacationHandler
{
    public class CreateVacationHandler : IRequestHandler<CreateVacationCommand, GeneralResponse>
    {
        private readonly IVacationRepository _vacationRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IGeneralRepositoryInterface<VacationType> _vacationTypeRepository;
        private readonly IMapper _mapper;

        public CreateVacationHandler(
            IVacationRepository vacationRepository,
            IEmployeeRepository employeeRepository,
            IGeneralRepositoryInterface<VacationType> vacationTypeRepository,
            IMapper mapper)
        {
            _vacationRepository = vacationRepository;
            _employeeRepository = employeeRepository;
            _vacationTypeRepository = vacationTypeRepository;
            _mapper = mapper;
        }

        public async Task<GeneralResponse> Handle(
            CreateVacationCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Vacation is null)
                return new GeneralResponse(false, "Vacation data is required.");

            if (request.Vacation.EmployeeId <= 0)
                return new GeneralResponse(false, "Employee ID is required.");

            // 1. التحقق من وجود الموظف
            var employee = await _employeeRepository.GetByIdAsync(request.Vacation.EmployeeId);
            if (employee is null)
                return new GeneralResponse(false, "Employee not found.");

            // 2. التحقق من وجود نوع الإجازة
            var vacationType = await _vacationTypeRepository.GetByIdAsync(request.Vacation.VacationTypeId);
            if (vacationType is null)
                return new GeneralResponse(false, "Selected Vacation Type does not exist.");

            // 3. التحقق من عدد الأيام
            if (request.Vacation.NumberOfDays <= 0)
                return new GeneralResponse(false, "Number of days must be at least 1.");

            // 4. التحويل عبر AutoMapper وحفظ الإجازة بحالة Pending
            var vacation = _mapper.Map<DomainVacation>(request.Vacation);
            vacation.Status = VacationStatus.Pending;
            vacation.CreatedAt = DateTime.UtcNow;
            vacation.ApprovedOrRejectedBy = null;
            vacation.DecisionDate = null;
            vacation.DecisionReason = null;
            return await _vacationRepository.InsertAsync(vacation);
        }
    }
}
