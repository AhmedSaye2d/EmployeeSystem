using Application.Service.Contracts.Logging;
using Domain.Entities;
using Infrastructure.DataAccess;
using Infrastructure.Services.Idempotency;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace EmployeeSystem.Tests
{
    public class IdempotencyTests
    {
        private AppDbContext CreateInMemoryDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task ExecuteIdempotently_NewMessage_ShouldExecuteAndRecordProcessedMessage()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            using var dbContext = CreateInMemoryDbContext(dbName);
            var loggerMock = new Mock<IAppLogger<IdempotentMessageService>>();
            var service = new IdempotentMessageService(dbContext, loggerMock.Object);

            var eventId = Guid.NewGuid();
            var executed = false;

            // Act
            var result = await service.ExecuteIdempotentlyAsync(
                eventId,
                "VacationApprovedEvent",
                "VacationApprovedConsumer",
                () =>
                {
                    executed = true;
                    return Task.CompletedTask;
                });

            // Assert
            Assert.True(result);
            Assert.True(executed);
            var processed = await dbContext.ProcessedMessages.FirstOrDefaultAsync(p => p.EventId == eventId);
            Assert.NotNull(processed);
            Assert.Equal(eventId, processed.EventId);
            Assert.Equal("VacationApprovedEvent", processed.MessageType);
        }

        [Fact]
        public async Task ExecuteIdempotently_DuplicateMessage_ShouldIgnoreAndNotReExecute()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            using var dbContext = CreateInMemoryDbContext(dbName);
            var loggerMock = new Mock<IAppLogger<IdempotentMessageService>>();
            var service = new IdempotentMessageService(dbContext, loggerMock.Object);

            var eventId = Guid.NewGuid();
            var executionCount = 0;

            // First execution
            await service.ExecuteIdempotentlyAsync(
                eventId,
                "VacationApprovedEvent",
                "VacationApprovedConsumer",
                () =>
                {
                    executionCount++;
                    return Task.CompletedTask;
                });

            // Act: duplicate execution with identical EventId
            var duplicateResult = await service.ExecuteIdempotentlyAsync(
                eventId,
                "VacationApprovedEvent",
                "VacationApprovedConsumer",
                () =>
                {
                    executionCount++;
                    return Task.CompletedTask;
                });

            // Assert
            Assert.False(duplicateResult);
            Assert.Equal(1, executionCount);
            loggerMock.Verify(l => l.LogWarning(It.Is<string>(msg => msg.Contains("Duplicate message detected"))), Times.AtLeastOnce);
        }

        [Fact]
        public async Task ExecuteIdempotently_FailedBusinessOperation_ShouldNotMarkAsProcessed()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            using var dbContext = CreateInMemoryDbContext(dbName);
            var loggerMock = new Mock<IAppLogger<IdempotentMessageService>>();
            var service = new IdempotentMessageService(dbContext, loggerMock.Object);

            var eventId = Guid.NewGuid();

            // Act & Assert: operation throws transient failure
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await service.ExecuteIdempotentlyAsync(
                    eventId,
                    "VacationApprovedEvent",
                    "VacationApprovedConsumer",
                    () => throw new InvalidOperationException("Email server timeout"));
            });

            // Assert that EventId was NOT recorded as processed
            var exists = await dbContext.ProcessedMessages.AnyAsync(p => p.EventId == eventId);
            Assert.False(exists);

            // Subsequent retry should now be allowed to execute successfully
            var retrySucceeded = await service.ExecuteIdempotentlyAsync(
                eventId,
                "VacationApprovedEvent",
                "VacationApprovedConsumer",
                () => Task.CompletedTask);

            Assert.True(retrySucceeded);
            var recordedAfterRetry = await dbContext.ProcessedMessages.AnyAsync(p => p.EventId == eventId);
            Assert.True(recordedAfterRetry);
        }

        [Fact]
        public async Task ExecuteIdempotently_ConcurrentDeliveries_ShouldOnlyExecuteOnce()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            using var dbContext = CreateInMemoryDbContext(dbName);
            var loggerMock = new Mock<IAppLogger<IdempotentMessageService>>();
            var service = new IdempotentMessageService(dbContext, loggerMock.Object);

            var eventId = Guid.NewGuid();
            var executionCount = 0;

            // Act: simulate 5 concurrent deliveries of the exact same event
            var tasks = Enumerable.Range(0, 5).Select(_ =>
                service.ExecuteIdempotentlyAsync(
                    eventId,
                    "VacationApprovedEvent",
                    "VacationApprovedConsumer",
                    async () =>
                    {
                        Interlocked.Increment(ref executionCount);
                        await Task.Delay(20);
                    })
            );

            var results = await Task.WhenAll(tasks);

            // Assert: exactly one delivery should have executed
            Assert.Equal(1, executionCount);
            Assert.Equal(1, results.Count(r => r == true));
            Assert.Equal(4, results.Count(r => r == false));
        }
    }
}
