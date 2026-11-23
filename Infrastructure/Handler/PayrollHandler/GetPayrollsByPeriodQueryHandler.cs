using Application.Dtos.Response.Payroll;
using Application.Service.Contracts;
using Application.Service.Payroll.Queries;
using AutoMapper;
using MediatR;

namespace Infrastructure.Handler.PayrollHandler
{
    public class GetPayrollsByPeriodQueryHandler : IRequestHandler<GetPayrollsByPeriodQuery, IEnumerable<PayrollResponse>>
    {
        private readonly IPayrollRepository _payrollRepository;
        private readonly IMapper _mapper;

        public GetPayrollsByPeriodQueryHandler(IPayrollRepository payrollRepository, IMapper mapper)
        {
            _payrollRepository = payrollRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PayrollResponse>> Handle(
            GetPayrollsByPeriodQuery request,
            CancellationToken cancellationToken)
        {
            var payrolls = await _payrollRepository.GetByPeriodAsync(request.Month, request.Year);
            return _mapper.Map<IEnumerable<PayrollResponse>>(payrolls);
        }
    }
}
