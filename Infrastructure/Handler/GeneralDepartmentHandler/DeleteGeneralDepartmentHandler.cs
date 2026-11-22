using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.GeneralDepartment.Commands;
using DomainGeneralDepartment = Domain.Entities.GeneralDepartment;
using MediatR;

namespace Infrastructure.Handler.GeneralDepartmentHandler
{
    public class DeleteGeneralDepartmentHandler : IRequestHandler<DeleteGeneralDepartmentCommand, GeneralResponse>
    {
        private readonly IGeneralRepositoryInterface<DomainGeneralDepartment> _generalDepartmentRepository;

        public DeleteGeneralDepartmentHandler(IGeneralRepositoryInterface<DomainGeneralDepartment> generalDepartmentRepository)
        {
            _generalDepartmentRepository = generalDepartmentRepository;
        }

        public async Task<GeneralResponse> Handle(
            DeleteGeneralDepartmentCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return new GeneralResponse(false, "Invalid General Department ID.");

            return await _generalDepartmentRepository.DeleteAsync(request.Id);
        }
    }
}
