using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.Sanction.Commands;
using AutoMapper;
using Domain.Entities;
using DomainSanction = Domain.Entities.Sanction;
using MediatR;

namespace Infrastructure.Handler.SanctionHandler
{
    public class CreateSanctionHandler : IRequestHandler<CreateSanctionCommand, GeneralResponse>
    {
        private readonly IGeneralRepositoryInterface<DomainSanction> _sanctionRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IGeneralRepositoryInterface<SanctionType> _sanctionTypeRepository;
        private readonly IMapper _mapper;

        public CreateSanctionHandler(
            IGeneralRepositoryInterface<DomainSanction> sanctionRepository,
            IEmployeeRepository employeeRepository,
            IGeneralRepositoryInterface<SanctionType> sanctionTypeRepository,
            IMapper mapper)
        {
            _sanctionRepository = sanctionRepository;
            _employeeRepository = employeeRepository;
            _sanctionTypeRepository = sanctionTypeRepository;
            _mapper = mapper;
        }

        public async Task<GeneralResponse> Handle(
            CreateSanctionCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Sanction is null)
                return new GeneralResponse(false, "Sanction data is required.");

            // 1. التحقق من وجود الموظف
            var employee = await _employeeRepository.GetByIdAsync(request.Sanction.EmployeeId);
            if (employee is null)
                return new GeneralResponse(false, "Employee not found.");

            // 2. التحقق من وجود نوع الجزاء
            var sanctionType = await _sanctionTypeRepository.GetByIdAsync(request.Sanction.SanctionTypeId);
            if (sanctionType is null)
                return new GeneralResponse(false, "Selected Sanction Type does not exist.");

            // 3. التحويل والحفظ
            var sanction = _mapper.Map<DomainSanction>(request.Sanction);
            return await _sanctionRepository.InsertAsync(sanction);
        }
    }
}
