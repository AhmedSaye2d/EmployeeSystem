using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.OverTime.Commands;
using Domain.Entities;
using DomainOverTime = Domain.Entities.OverTime;
using MediatR;

namespace Infrastructure.Handler.OverTimeHandler
{
    public class UpdateOverTimeHandler : IRequestHandler<UpdateOverTimeCommand, GeneralResponse>
    {
        private readonly IOverTimeRepository _overTimeRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IGeneralRepositoryInterface<OverTimeType> _overTimeTypeRepository;

        public UpdateOverTimeHandler(
            IOverTimeRepository overTimeRepository,
            IEmployeeRepository employeeRepository,
            IGeneralRepositoryInterface<OverTimeType> overTimeTypeRepository)
        {
            _overTimeRepository = overTimeRepository;
            _employeeRepository = employeeRepository;
            _overTimeTypeRepository = overTimeTypeRepository;
        }

        public async Task<GeneralResponse> Handle(
            UpdateOverTimeCommand request,
            CancellationToken cancellationToken)
        {
            if (request.OverTime is null || request.OverTime.Id <= 0)
                return new GeneralResponse(false, "Invalid overtime data or ID.");

            if (request.OverTime.EndDate < request.OverTime.StartDate)
                return new GeneralResponse(false, "End date cannot be earlier than start date.");

            // 1. التحقق من وجود سجل العمل الإضافي
            var overTime = await _overTimeRepository.GetByIdAsync(request.OverTime.Id);
            if (overTime is null)
                return new GeneralResponse(false, "Overtime record not found.");

            // 2. التحقق من وجود الموظف
            var employee = await _employeeRepository.GetByIdAsync(request.OverTime.EmployeeId);
            if (employee is null)
                return new GeneralResponse(false, "Employee not found.");

            // 3. التحقق من وجود نوع العمل الإضافي
            var overTimeType = await _overTimeTypeRepository.GetByIdAsync(request.OverTime.OverTimeTypeId);
            if (overTimeType is null)
                return new GeneralResponse(false, "Selected Overtime Type does not exist.");

            // 4. تحديث البيانات
            overTime.EmployeeId     = request.OverTime.EmployeeId;
            overTime.OverTimeTypeId = request.OverTime.OverTimeTypeId;
            overTime.StartDate      = request.OverTime.StartDate;
            overTime.EndDate        = request.OverTime.EndDate;
            overTime.Other          = request.OverTime.Other;

            return await _overTimeRepository.UpdateAsync(overTime);
        }
    }
}
