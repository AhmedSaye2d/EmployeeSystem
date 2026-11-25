using Application.Helper;
using Application.Service.Contracts.Email;
using Application.Service.Contracts.Events;
using Application.Service.Contracts.Idempotency;
using Application.Service.Contracts.Logging;
using Infrastructure.Consumers;
using MassTransit;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace EmployeeSystem.Tests
{
    public class ConsumerRetryAndDlqTests
    {
        private (IOptions<RabbitMqSettings>, Mock<IEmailService>, Mock<IIdempotentMessageService>, Mock<IAppLogger<VacationApprovedConsumer>>) SetupMocks()
        {
            var options = Options.Create(new RabbitMqSettings
            {
                Retry = new RabbitMqRetrySettings
                {
                    MaxRetryAttempts = 3,
                    InitialIntervalSeconds = 1,
                    IntervalIncrementSeconds = 1,
                    VacationApprovedQueue = "vacation-approved",
                    VacationRejectedQueue = "vacation-rejected",
                    DeadLetterExchange = "vacation-dead-letter-exchange",
                    DeadLetterQueue = "vacation-dead-letter-queue"
                }
            });

            var emailMock = new Mock<IEmailService>();
            var idempotencyMock = new Mock<IIdempotentMessageService>();
            var loggerMock = new Mock<IAppLogger<VacationApprovedConsumer>>();

            return (options, emailMock, idempotencyMock, loggerMock);
        }

        [Fact]
        public async Task VacationApprovedConsumer_SuccessfulProcessing_ShouldSendEmailAndAck()
        {
            // Arrange
            var (options, emailMock, idempotencyMock, loggerMock) = SetupMocks();

            idempotencyMock
                .Setup(i => i.ExecuteIdempotentlyAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Func<Task>>(),
                    It.IsAny<CancellationToken>()))
                .Returns<Guid, string, string, Func<Task>, CancellationToken>(async (id, type, consumer, op, ct) =>
                {
                    await op();
                    return true;
                });

            var consumer = new VacationApprovedConsumer(emailMock.Object, idempotencyMock.Object, options, loggerMock.Object);

            var contextMock = new Mock<ConsumeContext<VacationApprovedEvent>>();
            var eventId = Guid.NewGuid();
            var message = new VacationApprovedEvent(
                EventId: eventId,
                VacationId: 10,
                EmployeeId: 1,
                EmployeeName: "Ahmed",
                EmployeeEmail: "ahmed@example.com",
                VacationTypeName: "Annual",
                StartDate: DateTime.UtcNow,
                EndDate: DateTime.UtcNow.AddDays(3),
                NumberOfDays: 4,
                ApprovedBy: "Admin",
                ApprovedAt: DateTime.UtcNow
            );

            contextMock.SetupGet(c => c.Message).Returns(message);
            contextMock.SetupGet(c => c.CancellationToken).Returns(CancellationToken.None);

            // Act
            await consumer.Consume(contextMock.Object);

            // Assert
            emailMock.Verify(e => e.SendEmailAsync(
                "ahmed@example.com",
                It.Is<string>(s => s.Contains("Approved")),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()), Times.Once);

            loggerMock.Verify(l => l.LogInformation(It.Is<string>(msg => msg.Contains("Successfully sent email"))), Times.Once);
        }

        [Fact]
        public async Task VacationApprovedConsumer_DuplicateEvent_ShouldIgnoreAndAckWithoutResendingEmail()
        {
            // Arrange
            var (options, emailMock, idempotencyMock, loggerMock) = SetupMocks();

            idempotencyMock
                .Setup(i => i.ExecuteIdempotentlyAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Func<Task>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false); // Duplicate detected

            var consumer = new VacationApprovedConsumer(emailMock.Object, idempotencyMock.Object, options, loggerMock.Object);

            var contextMock = new Mock<ConsumeContext<VacationApprovedEvent>>();
            var eventId = Guid.NewGuid();
            var message = new VacationApprovedEvent(
                EventId: eventId,
                VacationId: 10,
                EmployeeId: 1,
                EmployeeName: "Ahmed",
                EmployeeEmail: "ahmed@example.com",
                VacationTypeName: "Annual",
                StartDate: DateTime.UtcNow,
                EndDate: DateTime.UtcNow.AddDays(3),
                NumberOfDays: 4,
                ApprovedBy: "Admin",
                ApprovedAt: DateTime.UtcNow
            );

            contextMock.SetupGet(c => c.Message).Returns(message);

            // Act
            await consumer.Consume(contextMock.Object);

            // Assert
            emailMock.Verify(e => e.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            loggerMock.Verify(l => l.LogInformation(It.Is<string>(msg => msg.Contains("Duplicate EventId") && msg.Contains("ignored and ACKed"))), Times.Once);
        }

        [Fact]
        public async Task VacationApprovedConsumer_TemporaryFailure_ShouldThrowToAllowRetry()
        {
            // Arrange
            var (options, emailMock, idempotencyMock, loggerMock) = SetupMocks();

            idempotencyMock
                .Setup(i => i.ExecuteIdempotentlyAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Func<Task>>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new TimeoutException("SMTP connection timed out"));

            var consumer = new VacationApprovedConsumer(emailMock.Object, idempotencyMock.Object, options, loggerMock.Object);

            var contextMock = new Mock<ConsumeContext<VacationApprovedEvent>>();
            var eventId = Guid.NewGuid();
            var message = new VacationApprovedEvent(
                EventId: eventId,
                VacationId: 10,
                EmployeeId: 1,
                EmployeeName: "Ahmed",
                EmployeeEmail: "ahmed@example.com",
                VacationTypeName: "Annual",
                StartDate: DateTime.UtcNow,
                EndDate: DateTime.UtcNow.AddDays(3),
                NumberOfDays: 4,
                ApprovedBy: "Admin",
                ApprovedAt: DateTime.UtcNow
            );

            contextMock.SetupGet(c => c.Message).Returns(message);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<TimeoutException>(() => consumer.Consume(contextMock.Object));
            Assert.Equal("SMTP connection timed out", ex.Message);

            loggerMock.Verify(l => l.LogError(
                It.IsAny<Exception>(),
                It.Is<string>(msg => msg.Contains("Failure processing EventId"))), Times.Once);
        }

        [Fact]
        public async Task VacationApprovedConsumer_SuccessfulProcessing_AfterRetry_ShouldSendEmail()
        {
            // Arrange
            var (options, emailMock, idempotencyMock, loggerMock) = SetupMocks();

            var attempts = 0;
            idempotencyMock
                .Setup(i => i.ExecuteIdempotentlyAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Func<Task>>(),
                    It.IsAny<CancellationToken>()))
                .Returns<Guid, string, string, Func<Task>, CancellationToken>(async (id, type, cons, op, ct) =>
                {
                    attempts++;
                    await op();
                    return true;
                });

            var consumer = new VacationApprovedConsumer(emailMock.Object, idempotencyMock.Object, options, loggerMock.Object);

            var contextMock = new Mock<ConsumeContext<VacationApprovedEvent>>();
            var eventId = Guid.NewGuid();
            var message = new VacationApprovedEvent(
                EventId: eventId,
                VacationId: 10,
                EmployeeId: 1,
                EmployeeName: "Ahmed",
                EmployeeEmail: "ahmed@example.com",
                VacationTypeName: "Annual",
                StartDate: DateTime.UtcNow,
                EndDate: DateTime.UtcNow.AddDays(3),
                NumberOfDays: 4,
                ApprovedBy: "Admin",
                ApprovedAt: DateTime.UtcNow
            );

            var headersMock = new Mock<Headers>();
            object retryObj = 2; // Simulating second retry attempt
            headersMock.Setup(h => h.TryGetHeader(MessageHeaders.FaultRetryCount, out retryObj)).Returns(true);
            contextMock.SetupGet(c => c.Headers).Returns(headersMock.Object);
            contextMock.SetupGet(c => c.Message).Returns(message);

            // Act
            await consumer.Consume(contextMock.Object);

            // Assert
            Assert.Equal(1, attempts);
            emailMock.Verify(e => e.SendEmailAsync(
                "ahmed@example.com",
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()), Times.Once);

            loggerMock.Verify(l => l.LogInformation(It.Is<string>(msg => msg.Contains("RetryAttempt: 2/3"))), Times.Once);
        }

        [Fact]
        public async Task VacationApprovedConsumer_MaxAttemptsExhausted_ShouldLogDlqTransfer()
        {
            // Arrange
            var (options, emailMock, idempotencyMock, loggerMock) = SetupMocks();

            idempotencyMock
                .Setup(i => i.ExecuteIdempotentlyAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Func<Task>>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new TimeoutException("Persistent connection failure"));

            var consumer = new VacationApprovedConsumer(emailMock.Object, idempotencyMock.Object, options, loggerMock.Object);

            var contextMock = new Mock<ConsumeContext<VacationApprovedEvent>>();
            var eventId = Guid.NewGuid();
            var message = new VacationApprovedEvent(
                EventId: eventId,
                VacationId: 10,
                EmployeeId: 1,
                EmployeeName: "Ahmed",
                EmployeeEmail: "ahmed@example.com",
                VacationTypeName: "Annual",
                StartDate: DateTime.UtcNow,
                EndDate: DateTime.UtcNow.AddDays(3),
                NumberOfDays: 4,
                ApprovedBy: "Admin",
                ApprovedAt: DateTime.UtcNow
            );

            // Mock headers to simulate MassTransit retry attempt header = 3 (MaxRetries)
            var headersMock = new Mock<Headers>();
            object retryHeaderObj = 3;
            headersMock.Setup(h => h.TryGetHeader(MessageHeaders.FaultRetryCount, out retryHeaderObj)).Returns(true);
            contextMock.SetupGet(c => c.Headers).Returns(headersMock.Object);
            contextMock.SetupGet(c => c.Message).Returns(message);

            // Act & Assert
            await Assert.ThrowsAsync<TimeoutException>(() => consumer.Consume(contextMock.Object));

            // Verify DLQ log
            loggerMock.Verify(l => l.LogError(
                It.IsAny<Exception>(),
                It.Is<string>(msg => msg.Contains("Routing to Dead Letter Queue (DLQ)"))), Times.Once);
        }

        [Fact]
        public async Task VacationRejectedConsumer_SuccessfulProcessing_ShouldSendRejectionEmail()
        {
            // Arrange
            var options = Options.Create(new RabbitMqSettings
            {
                Retry = new RabbitMqRetrySettings { MaxRetryAttempts = 3 }
            });

            var emailMock = new Mock<IEmailService>();
            var idempotencyMock = new Mock<IIdempotentMessageService>();
            var loggerMock = new Mock<IAppLogger<VacationRejectedConsumer>>();

            idempotencyMock
                .Setup(i => i.ExecuteIdempotentlyAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Func<Task>>(),
                    It.IsAny<CancellationToken>()))
                .Returns<Guid, string, string, Func<Task>, CancellationToken>(async (id, type, cons, op, ct) =>
                {
                    await op();
                    return true;
                });

            var consumer = new VacationRejectedConsumer(emailMock.Object, idempotencyMock.Object, options, loggerMock.Object);

            var contextMock = new Mock<ConsumeContext<VacationRejectedEvent>>();
            var eventId = Guid.NewGuid();
            var message = new VacationRejectedEvent(
                EventId: eventId,
                VacationId: 15,
                EmployeeId: 2,
                EmployeeName: "Sarah",
                EmployeeEmail: "sarah@example.com",
                VacationTypeName: "Sick",
                StartDate: DateTime.UtcNow,
                EndDate: DateTime.UtcNow.AddDays(2),
                NumberOfDays: 3,
                RejectedBy: "HR Manager",
                RejectedAt: DateTime.UtcNow,
                RejectionReason: "Insufficient balance"
            );

            contextMock.SetupGet(c => c.Message).Returns(message);

            // Act
            await consumer.Consume(contextMock.Object);

            // Assert
            emailMock.Verify(e => e.SendEmailAsync(
                "sarah@example.com",
                It.Is<string>(s => s.Contains("Rejected")),
                It.Is<string>(b => b.Contains("Insufficient balance")),
                It.IsAny<CancellationToken>()), Times.Once);

            loggerMock.Verify(l => l.LogInformation(It.Is<string>(msg => msg.Contains("Successfully sent rejection email"))), Times.Once);
        }
    }
}
