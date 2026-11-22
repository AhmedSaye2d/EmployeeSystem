using Application.Dtos.Response.Department;
using Application.Service.Contracts;
using Application.Service.Department.Queries;
using AutoMapper;
using DomainDepartment = Domain.Entities.Department;
using MediatR;

namespace Infrastructure.Handler.DepartmentHandler
{
    public class GetDepartmentByIdQueryHandler : IRequestHandler<GetDepartmentByIdQuery, DepartmentResponse?>
    {
        private readonly IGeneralRepositoryInterface<DomainDepartment> _departmentRepository;
        private readonly IMapper _mapper;

        public GetDepartmentByIdQueryHandler(
            IGeneralRepositoryInterface<DomainDepartment> departmentRepository,
            IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        public async Task<DepartmentResponse?> Handle(
            GetDepartmentByIdQuery request,
            CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return null;

            var department = await _departmentRepository.GetByIdAsync(request.Id);
            return department is null ? null : _mapper.Map<DepartmentResponse>(department);
        }
    }
}
