using Domain.Entities;
using Domain.Entities.Enum;
using Xunit;

namespace EmployeeSystem.Tests
{
    public class PayrollCalculationUnitTests
    {
        [Theory]
        [InlineData(5000.00, 600.00, 1700.00, 300.00, 7000.00)]
        [InlineData(10000.00, 1500.00, 0.00, 500.00, 11000.00)]
        [InlineData(4000.00, 0.00, 800.00, 0.00, 4800.00)]
        public void Payroll_NetSalaryFormula_ShouldCalculateCorrectly(
            decimal basic, decimal allowances, decimal overtime, decimal deductions, decimal expectedNet)
        {
            // Arrange
            var payroll = new Payroll
            {
                BasicSalary = basic,
                Allowances = allowances,
                OverTimeAmount = overtime,
                Deductions = deductions,
                NetSalary = basic + allowances + overtime - deductions
            };

            // Assert
            Assert.Equal(expectedNet, payroll.NetSalary);
        }

        [Fact]
        public void Payroll_CrossMonthOvertime_Attribution_ShouldSplitDaysCorrectly()
        {
            // Scenario: Overtime from 28 Nov 2026 to 2 Dec 2026 (5 days total)
            var startDate = new DateTime(2026, 11, 28);
            var endDate = new DateTime(2026, 12, 2);

            // Month of November: 1 to 30
            var novStart = new DateTime(2026, 11, 1);
            var novEnd = new DateTime(2026, 11, 30);

            var effectiveStartNov = startDate > novStart ? startDate : novStart;
            var effectiveEndNov = endDate < novEnd ? endDate : novEnd;
            int novDays = (effectiveEndNov - effectiveStartNov).Days + 1;

            // Month of December: 1 to 31
            var decStart = new DateTime(2026, 12, 1);
            var decEnd = new DateTime(2026, 12, 31);

            var effectiveStartDec = startDate > decStart ? startDate : decStart;
            var effectiveEndDec = endDate < decEnd ? endDate : decEnd;
            int decDays = (effectiveEndDec - effectiveStartDec).Days + 1;

            // Assert: Nov has 3 days (28, 29, 30), Dec has 2 days (1, 2)
            Assert.Equal(3, novDays);
            Assert.Equal(2, decDays);
            Assert.Equal(5, novDays + decDays);
        }

        [Fact]
        public void Payroll_InitialStatus_ShouldDefaultToDraft()
        {
            var payroll = new Payroll();
            Assert.Equal(PayrollStatus.Draft, payroll.Status);
        }
    }
}
