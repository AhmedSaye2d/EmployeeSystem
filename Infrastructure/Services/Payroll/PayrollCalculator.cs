using Application.Service.Contracts;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Payroll
{
    public class PayrollCalculator : IPayrollCalculator
    {
        private readonly AppDbContext _context;

        public PayrollCalculator(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(decimal OverTimeAmount, decimal Deductions, decimal NetSalary, string? Warning)> CalculateAsync(
            int employeeId,
            decimal basicSalary,
            decimal allowances,
            int month,
            int year,
            CancellationToken cancellationToken = default)
        {
            // 1. تحديد نافذة الشهر المطلوب للاحتساب
            var monthStart = new DateTime(year, month, 1);
            var monthEnd = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            // 2. احتساب الإضافي للأيام الواقعة فعلياً داخل شهر المسير (Cross-Month Inclusive Calculation)
            var overtimes = await _context.OverTimes
                .Include(o => o.OverTimeType)
                .AsNoTracking()
                .Where(o => o.EmployeeId == employeeId && o.StartDate.Date <= monthEnd && o.EndDate.Date >= monthStart)
                .ToListAsync(cancellationToken);

            decimal totalOverTimeAmount = 0m;
            foreach (var o in overtimes)
            {
                var effectiveStart = o.StartDate.Date > monthStart ? o.StartDate.Date : monthStart;
                var effectiveEnd = o.EndDate.Date < monthEnd ? o.EndDate.Date : monthEnd;
                int daysInThisMonth = (effectiveEnd - effectiveStart).Days + 1;

                if (daysInThisMonth > 0)
                {
                    decimal rate = o.OverTimeType?.RatePerDay ?? 0m;
                    totalOverTimeAmount += daysInThisMonth * rate;
                }
            }

            // 3. احتساب الجزاءات بناءً على تاريخ قرار العقوبة (PunishmentDate) وتجاهل الصفرية
            var sanctions = await _context.Sanctions
                .AsNoTracking()
                .Where(s => s.EmployeeId == employeeId
                            && s.PunishmentDate.Month == month
                            && s.PunishmentDate.Year == year
                            && s.DeductionAmount > 0)
                .ToListAsync(cancellationToken);

            decimal totalDeductions = sanctions.Sum(s => s.DeductionAmount);

            // 4. احتساب صافي الراتب: NetSalary = BasicSalary + Allowances + Overtime - Deductions
            decimal gross = basicSalary + allowances + totalOverTimeAmount;
            decimal net = gross - totalDeductions;
            string? warning = null;

            // 5. تطبيق الخيار (ج): إذا كانت الخصومات أكبر من المستحقات، نرفق تنبيهاً واضحاً مع المسودة
            if (totalDeductions > gross)
            {
                warning = $"WARNING: Deductions ({totalDeductions:F2}) exceed gross earnings ({gross:F2}) by {totalDeductions - gross:F2}. Created as Draft for management review.";
            }

            return (totalOverTimeAmount, totalDeductions, net, warning);
        }
    }
}
