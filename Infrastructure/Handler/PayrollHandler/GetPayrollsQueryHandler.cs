using Application.Dtos.Response.Payroll;
using Application.Service.Contracts;
using Application.Service.Payroll.Queries;
using AutoMapper;
using MediatR;

namespace Infrastructure.Handler.PayrollHandler
{
    public class GetPayrollsQueryHandler : IRequestHandler<GetPayrollsQuery, IEnumerable<PayrollResponse>>
    {
        private readonly IPayrollRepository _payrollRepository;
        private readonly IMapper _mapper;

        public GetPayrollsQueryHandler(IPayrollRepository payrollRepository, IMapper mapper)
        {
            _payrollRepository = payrollRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PayrollResponse>> Handle(
            GetPayrollsQuery request,
            CancellationToken cancellationToken)
        {
            var payrolls = await _payrollRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PayrollResponse>>(payrolls);
        }
    }
}
