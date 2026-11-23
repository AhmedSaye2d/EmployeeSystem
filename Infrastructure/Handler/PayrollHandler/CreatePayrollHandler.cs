using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.Payroll.Commands;
using Domain.Entities.Enum;
using MediatR;
using DomainPayroll = Domain.Entities.Payroll;

namespace Infrastructure.Handler.PayrollHandler
{
    public class CreatePayrollHandler : IRequestHandler<CreatePayrollCommand, GeneralResponse>
    {
        private readonly IPayrollRepository _payrollRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPayrollCalculator _payrollCalculator;

        public CreatePayrollHandler(
            IPayrollRepository payrollRepository,
            IEmployeeRepository employeeRepository,
            IPayrollCalculator payrollCalculator)
        {
            _payrollRepository = payrollRepository;
            _employeeRepository = employeeRepository;
            _payrollCalculator = payrollCalculator;
        }

        public async Task<GeneralResponse> Handle(
            CreatePayrollCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Request is null)
                return new GeneralResponse(false, "Payroll request data is required.");

            // 1. التحقق من وجود الموظف
            var employee = await _employeeRepository.GetByIdAsync(request.Request.EmployeeId);
            if (employee is null)
                return new GeneralResponse(false, $"Employee with ID {request.Request.EmployeeId} not found.");

            // 2. التحقق من عدم وجود مسير سابق لنفس الموظف والشهر والسنة (منع التكرار)
            bool exists = await _payrollRepository.ExistsAsync(
                request.Request.EmployeeId,
                request.Request.Month,
                request.Request.Year);

            if (exists)
                return new GeneralResponse(false, $"Payroll for Employee #{request.Request.EmployeeId} for {request.Request.Month}/{request.Request.Year} already exists.");

            // 3. الاحتساب التلقائي للإضافي والخصومات وصافي الراتب
            var (overTimeAmount, deductions, netSalary, warning) = await _payrollCalculator.CalculateAsync(
                employee.Id,
                employee.BasicSalary,
                request.Request.Allowances,
                request.Request.Month,
                request.Request.Year,
                cancellationToken);

            // 4. دمج التنبيه مع الملاحظات في حال تجاوز الخصومات للمستحقات
            var notes = request.Request.Notes;
            if (!string.IsNullOrWhiteSpace(warning))
            {
                notes = string.IsNullOrWhiteSpace(notes) ? warning : $"{notes} | {warning}";
            }

            // 5. إنشاء سجل المسير كلقطة تاريخية دائمة (Historical Snapshot)
            var payroll = new DomainPayroll
            {
                EmployeeId = employee.Id,
                Month = request.Request.Month,
                Year = request.Request.Year,
                BasicSalary = employee.BasicSalary,
                Allowances = request.Request.Allowances,
                OverTimeAmount = overTimeAmount,
                Deductions = deductions,
                NetSalary = netSalary,
                Status = PayrollStatus.Draft,
                CreatedAt = DateTime.UtcNow,
                Notes = notes
            };

            var result = await _payrollRepository.InsertAsync(payroll);
            if (result.Flag && !string.IsNullOrWhiteSpace(warning))
            {
                return new GeneralResponse(true, $"Payroll draft created successfully. {warning}");
            }

            return result;
        }
    }
}
