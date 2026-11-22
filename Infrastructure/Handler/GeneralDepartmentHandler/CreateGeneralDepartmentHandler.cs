using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.GeneralDepartment.Commands;
using AutoMapper;
using DomainGeneralDepartment = Domain.Entities.GeneralDepartment;
using MediatR;

namespace Infrastructure.Handler.GeneralDepartmentHandler
{
    public class CreateGeneralDepartmentHandler : IRequestHandler<CreateGeneralDepartmentCommand, GeneralResponse>
    {
        private readonly IGeneralRepositoryInterface<DomainGeneralDepartment> _generalDepartmentRepository;
        private readonly IMapper _mapper;

        public CreateGeneralDepartmentHandler(
            IGeneralRepositoryInterface<DomainGeneralDepartment> generalDepartmentRepository,
            IMapper mapper)
        {
            _generalDepartmentRepository = generalDepartmentRepository;
            _mapper = mapper;
        }

        public async Task<GeneralResponse> Handle(
            CreateGeneralDepartmentCommand request,
            CancellationToken cancellationToken)
        {
            if (request.GeneralDepartment is null)
                return new GeneralResponse(false, "General Department data is required.");

            if (string.IsNullOrWhiteSpace(request.GeneralDepartment.Name))
                return new GeneralResponse(false, "General Department name is required.");

            var generalDepartment = _mapper.Map<DomainGeneralDepartment>(request.GeneralDepartment);
            return await _generalDepartmentRepository.InsertAsync(generalDepartment);
        }
    }
}
