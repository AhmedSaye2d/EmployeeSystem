using Application.Dtos.Response.Employee;
using Application.Service.Contracts;
using Application.Service.Employee.Queries;
using AutoMapper;
using MediatR;

namespace Infrastructure.Handler.EmployeeHandler
{
    public class SearchEmployeesQueryHandler : IRequestHandler<SearchEmployeesQuery, List<EmployeeResponse>>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public SearchEmployeesQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<List<EmployeeResponse>> Handle(
            SearchEmployeesQuery request,
            CancellationToken cancellationToken)
        {
            var employees = await _employeeRepository.SearchAsync(request.SearchTerm);
            return _mapper.Map<List<EmployeeResponse>>(employees);
        }
    }
}
