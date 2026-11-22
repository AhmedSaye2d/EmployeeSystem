using Application.Dtos.Response.Department;
using Application.Service.Contracts;
using Application.Service.Department.Queries;
using AutoMapper;
using DomainDepartment = Domain.Entities.Department;
using MediatR;

namespace Infrastructure.Handler.DepartmentHandler
{
    public class GetDepartmentsQueryHandler : IRequestHandler<GetDepartmentsQuery, List<DepartmentResponse>>
    {
        private readonly IGeneralRepositoryInterface<DomainDepartment> _departmentRepository;
        private readonly IMapper _mapper;

        public GetDepartmentsQueryHandler(
            IGeneralRepositoryInterface<DomainDepartment> departmentRepository,
            IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        public async Task<List<DepartmentResponse>> Handle(
            GetDepartmentsQuery request,
            CancellationToken cancellationToken)
        {
            var departments = await _departmentRepository.GetAllAsync();
            return _mapper.Map<List<DepartmentResponse>>(departments);
        }
    }
}
