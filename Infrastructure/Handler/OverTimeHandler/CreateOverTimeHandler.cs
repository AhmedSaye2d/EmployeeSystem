using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.OverTime.Commands;
using AutoMapper;
using Domain.Entities;
using DomainOverTime = Domain.Entities.OverTime;
using MediatR;

namespace Infrastructure.Handler.OverTimeHandler
{
    public class CreateOverTimeHandler : IRequestHandler<CreateOverTimeCommand, GeneralResponse>
    {
        private readonly IOverTimeRepository _overTimeRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IGeneralRepositoryInterface<OverTimeType> _overTimeTypeRepository;
        private readonly IMapper _mapper;

        public CreateOverTimeHandler(
            IOverTimeRepository overTimeRepository,
            IEmployeeRepository employeeRepository,
            IGeneralRepositoryInterface<OverTimeType> overTimeTypeRepository,
            IMapper mapper)
        {
            _overTimeRepository = overTimeRepository;
            _employeeRepository = employeeRepository;
            _overTimeTypeRepository = overTimeTypeRepository;
            _mapper = mapper;
        }

        public async Task<GeneralResponse> Handle(
            CreateOverTimeCommand request,
            CancellationToken cancellationToken)
        {
            if (request.OverTime is null)
                return new GeneralResponse(false, "Overtime data is required.");

            if (request.OverTime.EndDate < request.OverTime.StartDate)
                return new GeneralResponse(false, "End date cannot be earlier than start date.");

            // 1. التحقق من وجود الموظف
            var employee = await _employeeRepository.GetByIdAsync(request.OverTime.EmployeeId);
            if (employee is null)
                return new GeneralResponse(false, "Employee not found.");

            // 2. التحقق من وجود نوع العمل الإضافي
            var overTimeType = await _overTimeTypeRepository.GetByIdAsync(request.OverTime.OverTimeTypeId);
            if (overTimeType is null)
                return new GeneralResponse(false, "Selected Overtime Type does not exist.");

            // 3. التحويل والحفظ
            var overTime = _mapper.Map<DomainOverTime>(request.OverTime);
            return await _overTimeRepository.InsertAsync(overTime);
        }
    }
}
