using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.GeneralDepartment.Commands;
using DomainGeneralDepartment = Domain.Entities.GeneralDepartment;
using MediatR;

namespace Infrastructure.Handler.GeneralDepartmentHandler
{
    public class UpdateGeneralDepartmentHandler : IRequestHandler<UpdateGeneralDepartmentCommand, GeneralResponse>
    {
        private readonly IGeneralRepositoryInterface<DomainGeneralDepartment> _generalDepartmentRepository;

        public UpdateGeneralDepartmentHandler(IGeneralRepositoryInterface<DomainGeneralDepartment> generalDepartmentRepository)
        {
            _generalDepartmentRepository = generalDepartmentRepository;
        }

        public async Task<GeneralResponse> Handle(
            UpdateGeneralDepartmentCommand request,
            CancellationToken cancellationToken)
        {
            if (request.GeneralDepartment is null || request.GeneralDepartment.Id <= 0)
                return new GeneralResponse(false, "Invalid General Department data or ID.");

            if (string.IsNullOrWhiteSpace(request.GeneralDepartment.Name))
                return new GeneralResponse(false, "General Department name is required.");

            var generalDepartment = await _generalDepartmentRepository.GetByIdAsync(request.GeneralDepartment.Id);
            if (generalDepartment is null)
                return new GeneralResponse(false, "General Department not found.");

            generalDepartment.Name = request.GeneralDepartment.Name.Trim();
            return await _generalDepartmentRepository.UpdateAsync(generalDepartment);
        }
    }
}
