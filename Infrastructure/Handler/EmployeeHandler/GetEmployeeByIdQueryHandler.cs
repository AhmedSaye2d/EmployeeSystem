using Application.Dtos.Response.Employee;
using Application.Service.Contracts;
using Application.Service.Employee.Queries;
using AutoMapper;
using MediatR;

namespace Infrastructure.Handler.EmployeeHandler
{
    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDetailsResponse?>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public GetEmployeeByIdQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<EmployeeDetailsResponse?> Handle(
            GetEmployeeByIdQuery request,
            CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return null;

            var employee = await _employeeRepository.GetByIdAsync(request.Id);
            return employee is null ? null : _mapper.Map<EmployeeDetailsResponse>(employee);
        }
    }
}
