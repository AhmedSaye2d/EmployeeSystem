using Application.Dtos.Response.Payroll;
using Application.Service.Contracts;
using Application.Service.Payroll.Queries;
using AutoMapper;
using MediatR;

namespace Infrastructure.Handler.PayrollHandler
{
    public class GetPayrollByIdQueryHandler : IRequestHandler<GetPayrollByIdQuery, PayrollResponse?>
    {
        private readonly IPayrollRepository _payrollRepository;
        private readonly IMapper _mapper;

        public GetPayrollByIdQueryHandler(IPayrollRepository payrollRepository, IMapper mapper)
        {
            _payrollRepository = payrollRepository;
            _mapper = mapper;
        }

        public async Task<PayrollResponse?> Handle(
            GetPayrollByIdQuery request,
            CancellationToken cancellationToken)
        {
            var payroll = await _payrollRepository.GetByIdAsync(request.Id);
            return payroll is null ? null : _mapper.Map<PayrollResponse>(payroll);
        }
    }
}
