using Application.Helper;
using Application.Service.Contracts.Email;
using Application.Service.Contracts.Events;
using Application.Service.Contracts.Idempotency;
using Application.Service.Contracts.Logging;
using MassTransit;
using Microsoft.Extensions.Options;

namespace Infrastructure.Consumers
{
    public class VacationApprovedConsumer(
        IEmailService emailService,
        IIdempotentMessageService idempotencyService,
        IOptions<RabbitMqSettings> rabbitOptions,
        IAppLogger<VacationApprovedConsumer> logger) : IConsumer<VacationApprovedEvent>
    {
        public async Task Consume(ConsumeContext<VacationApprovedEvent> context)
        {
            var message = context.Message;
            var retryAttempt = context.GetRetryAttempt();
            if (retryAttempt == 0 && context.Headers != null)
            {
                if (context.Headers.TryGetHeader(MessageHeaders.FaultRetryCount, out var faultRetryObj) && faultRetryObj is int faultCount)
                {
                    retryAttempt = faultCount;
                }
                else if (context.Headers.TryGetHeader(MessageHeaders.RedeliveryCount, out var redeliveryObj) && redeliveryObj is int redeliveryCount)
                {
                    retryAttempt = redeliveryCount;
                }
            }

            var maxRetries = rabbitOptions.Value.Retry.MaxRetryAttempts;

            logger.LogInformation(
                $"[VacationApprovedConsumer] Processing approval event for EventId: {message.EventId}, " +
                $"VacationId: {message.VacationId}, RetryAttempt: {retryAttempt}/{maxRetries}");

            try
            {
                var executed = await idempotencyService.ExecuteIdempotentlyAsync(
                    eventId: message.EventId,
                    messageType: nameof(VacationApprovedEvent),
                    consumerName: nameof(VacationApprovedConsumer),
                    businessOperation: async () =>
                    {
                        var subject = $"Vacation Request Approved - {message.VacationTypeName}";
                        var body = $"""
                            <h3>Dear {message.EmployeeName},</h3>
                            <p>We are pleased to inform you that your vacation request has been <strong>APPROVED</strong>.</p>
                            <table style="border-collapse: collapse; width: 100%; max-width: 500px;">
                                <tr><td style="padding: 8px; border: 1px solid #ddd;"><strong>Vacation Type:</strong></td><td style="padding: 8px; border: 1px solid #ddd;">{message.VacationTypeName}</td></tr>
                                <tr><td style="padding: 8px; border: 1px solid #ddd;"><strong>Start Date:</strong></td><td style="padding: 8px; border: 1px solid #ddd;">{message.StartDate:yyyy-MM-dd}</td></tr>
                                <tr><td style="padding: 8px; border: 1px solid #ddd;"><strong>End Date:</strong></td><td style="padding: 8px; border: 1px solid #ddd;">{message.EndDate:yyyy-MM-dd}</td></tr>
                                <tr><td style="padding: 8px; border: 1px solid #ddd;"><strong>Number of Days:</strong></td><td style="padding: 8px; border: 1px solid #ddd;">{message.NumberOfDays}</td></tr>
                                <tr><td style="padding: 8px; border: 1px solid #ddd;"><strong>Approved By:</strong></td><td style="padding: 8px; border: 1px solid #ddd;">{message.ApprovedBy}</td></tr>
                                <tr><td style="padding: 8px; border: 1px solid #ddd;"><strong>Approved Date:</strong></td><td style="padding: 8px; border: 1px solid #ddd;">{message.ApprovedAt:yyyy-MM-dd HH:mm}</td></tr>
                            </table>
                            <br/>
                            <p>Best regards,<br/>Employee Management System</p>
                            """;

                        await emailService.SendEmailAsync(message.EmployeeEmail, subject, body, context.CancellationToken);
                    },
                    cancellationToken: context.CancellationToken);

                if (executed)
                {
                    logger.LogInformation($"[VacationApprovedConsumer] Successfully sent email and completed EventId: {message.EventId} for VacationId: {message.VacationId}.");
                }
                else
                {
                    logger.LogInformation($"[VacationApprovedConsumer] Duplicate EventId: {message.EventId} for VacationId: {message.VacationId} ignored and ACKed.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    $"[VacationApprovedConsumer] Failure processing EventId: {message.EventId}, " +
                    $"VacationId: {message.VacationId}, RetryAttempt: {retryAttempt}/{maxRetries}. Error: {ex.Message}");

                if (retryAttempt >= maxRetries)
                {
                    logger.LogError(
                        ex,
                        $"[VacationApprovedConsumer] Maximum retries ({maxRetries}) exhausted for EventId: {message.EventId}. " +
                        $"Routing to Dead Letter Queue (DLQ).");
                }

                throw; // Rethrow to trigger MassTransit retry or move to DLQ
            }
        }
    }
}
