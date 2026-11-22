using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.Sanction.Commands;
using Domain.Entities;
using DomainSanction = Domain.Entities.Sanction;
using MediatR;

namespace Infrastructure.Handler.SanctionHandler
{
    public class UpdateSanctionHandler : IRequestHandler<UpdateSanctionCommand, GeneralResponse>
    {
        private readonly IGeneralRepositoryInterface<DomainSanction> _sanctionRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IGeneralRepositoryInterface<SanctionType> _sanctionTypeRepository;

        public UpdateSanctionHandler(
            IGeneralRepositoryInterface<DomainSanction> sanctionRepository,
            IEmployeeRepository employeeRepository,
            IGeneralRepositoryInterface<SanctionType> sanctionTypeRepository)
        {
            _sanctionRepository = sanctionRepository;
            _employeeRepository = employeeRepository;
            _sanctionTypeRepository = sanctionTypeRepository;
        }

        public async Task<GeneralResponse> Handle(
            UpdateSanctionCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Sanction is null || request.Sanction.Id <= 0)
                return new GeneralResponse(false, "Invalid sanction data or ID.");

            // 1. التحقق من وجود الجزاء
            var sanction = await _sanctionRepository.GetByIdAsync(request.Sanction.Id);
            if (sanction is null)
                return new GeneralResponse(false, "Sanction not found.");

            // 2. التحقق من وجود الموظف
            var employee = await _employeeRepository.GetByIdAsync(request.Sanction.EmployeeId);
            if (employee is null)
                return new GeneralResponse(false, "Employee not found.");

            // 3. التحقق من وجود نوع الجزاء
            var sanctionType = await _sanctionTypeRepository.GetByIdAsync(request.Sanction.SanctionTypeId);
            if (sanctionType is null)
                return new GeneralResponse(false, "Selected Sanction Type does not exist.");

            // 4. تحديث البيانات
            sanction.EmployeeId     = request.Sanction.EmployeeId;
            sanction.SanctionTypeId = request.Sanction.SanctionTypeId;
            sanction.Data           = request.Sanction.Data;
            sanction.Punishment     = request.Sanction.Punishment;
            sanction.PunishmentDate = request.Sanction.PunishmentDate;
            sanction.Other          = request.Sanction.Other;

            return await _sanctionRepository.UpdateAsync(sanction);
        }
    }
}
