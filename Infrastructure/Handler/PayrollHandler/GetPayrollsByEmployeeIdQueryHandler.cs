using Application.Dtos.Response.Payroll;
using Application.Service.Contracts;
using Application.Service.Payroll.Queries;
using AutoMapper;
using MediatR;

namespace Infrastructure.Handler.PayrollHandler
{
    public class GetPayrollsByEmployeeIdQueryHandler : IRequestHandler<GetPayrollsByEmployeeIdQuery, IEnumerable<PayrollResponse>>
    {
        private readonly IPayrollRepository _payrollRepository;
        private readonly IMapper _mapper;

        public GetPayrollsByEmployeeIdQueryHandler(IPayrollRepository payrollRepository, IMapper mapper)
        {
            _payrollRepository = payrollRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PayrollResponse>> Handle(
            GetPayrollsByEmployeeIdQuery request,
            CancellationToken cancellationToken)
        {
            var payrolls = await _payrollRepository.GetByEmployeeIdAsync(request.EmployeeId);
            return _mapper.Map<IEnumerable<PayrollResponse>>(payrolls);
        }
    }
}
