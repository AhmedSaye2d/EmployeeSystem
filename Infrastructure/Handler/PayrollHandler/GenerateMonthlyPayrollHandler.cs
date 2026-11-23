using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.Payroll.Commands;
using Domain.Entities.Enum;
using MediatR;
using DomainPayroll = Domain.Entities.Payroll;

namespace Infrastructure.Handler.PayrollHandler
{
    public class GenerateMonthlyPayrollHandler : IRequestHandler<GenerateMonthlyPayrollCommand, GeneralResponse>
    {
        private readonly IPayrollRepository _payrollRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPayrollCalculator _payrollCalculator;

        public GenerateMonthlyPayrollHandler(
            IPayrollRepository payrollRepository,
            IEmployeeRepository employeeRepository,
            IPayrollCalculator payrollCalculator)
        {
            _payrollRepository = payrollRepository;
            _employeeRepository = employeeRepository;
            _payrollCalculator = payrollCalculator;
        }

        public async Task<GeneralResponse> Handle(
            GenerateMonthlyPayrollCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Request is null)
                return new GeneralResponse(false, "Generate request is required.");

            // 1. جلب الموظفين المستهدفين
            var allEmployees = await _employeeRepository.GetAllAsync();
            if (request.Request.EmployeeIds != null && request.Request.EmployeeIds.Count > 0)
            {
                allEmployees = allEmployees.Where(e => request.Request.EmployeeIds.Contains(e.Id));
            }

            var employeesList = allEmployees.ToList();
            if (employeesList.Count == 0)
                return new GeneralResponse(false, "No matching employees found to generate payroll.");

            int createdCount = 0;
            int skippedCount = 0;
            var warnings = new List<string>();

            // 2. معالجة كل موظف على حدة واحتساب مستحقاته وخصوماته بدقة
            foreach (var employee in employeesList)
            {
                // منع التكرار: إذا وُجد مسير مسبق لهذا الشهر والسنة، يتم تخطيه
                bool exists = await _payrollRepository.ExistsAsync(employee.Id, request.Request.Month, request.Request.Year);
                if (exists)
                {
                    skippedCount++;
                    continue;
                }

                var (overTimeAmount, deductions, netSalary, warning) = await _payrollCalculator.CalculateAsync(
                    employee.Id,
                    employee.BasicSalary,
                    0m, // البدلات الافتراضية للتوليد المجمع = 0
                    request.Request.Month,
                    request.Request.Year,
                    cancellationToken);

                var notes = request.Request.Notes;
                if (!string.IsNullOrWhiteSpace(warning))
                {
                    notes = string.IsNullOrWhiteSpace(notes) ? warning : $"{notes} | {warning}";
                    warnings.Add($"Employee #{employee.Id} ({employee.Name}): {warning}");
                }

                var payroll = new DomainPayroll
                {
                    EmployeeId = employee.Id,
                    Month = request.Request.Month,
                    Year = request.Request.Year,
                    BasicSalary = employee.BasicSalary,
                    Allowances = 0m,
                    OverTimeAmount = overTimeAmount,
                    Deductions = deductions,
                    NetSalary = netSalary,
                    Status = PayrollStatus.Draft,
                    CreatedAt = DateTime.UtcNow,
                    Notes = notes
                };

                var insertResult = await _payrollRepository.InsertAsync(payroll);
                if (insertResult.Flag)
                {
                    createdCount++;
                }
            }

            string summary = $"Monthly Payroll generation completed for {request.Request.Month}/{request.Request.Year}. Created: {createdCount}, Skipped (Already existed): {skippedCount}.";
            if (warnings.Count > 0)
            {
                summary += $" {warnings.Count} warning(s) logged for high deductions.";
            }

            return new GeneralResponse(true, summary);
        }
    }
}
