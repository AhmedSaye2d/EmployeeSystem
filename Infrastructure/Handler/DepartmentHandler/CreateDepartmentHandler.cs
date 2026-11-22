using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.Department.Commands;
using AutoMapper;
using Domain.Entities;
using DomainDepartment = Domain.Entities.Department;
using MediatR;

namespace Infrastructure.Handler.DepartmentHandler
{
    public class CreateDepartmentHandler : IRequestHandler<CreateDepartmentCommand, GeneralResponse>
    {
        private readonly IGeneralRepositoryInterface<DomainDepartment> _departmentRepository;
        private readonly IGeneralRepositoryInterface<GeneralDepartment> _generalDepartmentRepository;
        private readonly IMapper _mapper;

        public CreateDepartmentHandler(
            IGeneralRepositoryInterface<DomainDepartment> departmentRepository,
            IGeneralRepositoryInterface<GeneralDepartment> generalDepartmentRepository,
            IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _generalDepartmentRepository = generalDepartmentRepository;
            _mapper = mapper;
        }

        public async Task<GeneralResponse> Handle(
            CreateDepartmentCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Department is null)
                return new GeneralResponse(false, "Department data is required.");

            if (string.IsNullOrWhiteSpace(request.Department.Name))
                return new GeneralResponse(false, "Department name is required.");

            if (request.Department.GeneralDepartmentId <= 0)
                return new GeneralResponse(false, "General Department is required.");

            var generalDept = await _generalDepartmentRepository.GetByIdAsync(request.Department.GeneralDepartmentId);
            if (generalDept is null)
                return new GeneralResponse(false, "Selected General Department does not exist.");

            var department = _mapper.Map<DomainDepartment>(request.Department);
            return await _departmentRepository.InsertAsync(department);
        }
    }
}
