using Domain.Entities;
using Xunit;

namespace EmployeeSystem.Tests
{
    public class VacationAndOvertimeUnitTests
    {
        [Fact]
        public void OverTime_NumberOfDays_ShouldBeInclusive_WhenSingleDay()
        {
            // Arrange
            var overTime = new OverTime
            {
                StartDate = new DateTime(2026, 11, 1),
                EndDate = new DateTime(2026, 11, 1)
            };

            // Act & Assert: Single day should equal 1 day, not 0
            Assert.Equal(1, overTime.NumberOfDays);
        }

        [Fact]
        public void OverTime_NumberOfDays_ShouldBeInclusive_WhenMultiDay()
        {
            // Arrange
            var overTime = new OverTime
            {
                StartDate = new DateTime(2026, 11, 10),
                EndDate = new DateTime(2026, 11, 12)
            };

            // Act & Assert: 10, 11, 12 = 3 days
            Assert.Equal(3, overTime.NumberOfDays);
        }

        [Fact]
        public void Vacation_EndDate_ShouldMatchNumberOfDays()
        {
            // Arrange
            var vacation = new Vacation
            {
                StartData = new DateTime(2026, 11, 1),
                NumberOfDays = 5
            };

            // Act & Assert: 1st Nov + 4 days = 5th Nov
            Assert.Equal(new DateTime(2026, 11, 5), vacation.EndData);
        }
    }
}
