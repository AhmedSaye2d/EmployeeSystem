using Application.Dtos.Response.Employee;
using Application.Service.Contracts;
using Application.Service.Employee.Queries;
using AutoMapper;
using MediatR;

namespace Infrastructure.Handler.EmployeeHandler
{
    public class GetAllEmployeeHandler : IRequestHandler<GetEmployeesQuery, List<EmployeeResponse>>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public GetAllEmployeeHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<List<EmployeeResponse>> Handle(
            GetEmployeesQuery request,
            CancellationToken cancellationToken)
        {
            var employees = await _employeeRepository.GetAllAsync();
            return _mapper.Map<List<EmployeeResponse>>(employees);
        }
    }
}
