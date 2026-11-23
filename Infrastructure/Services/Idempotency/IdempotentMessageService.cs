using System.Collections.Concurrent;
using Application.Service.Contracts.Idempotency;
using Application.Service.Contracts.Logging;
using Domain.Entities;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Idempotency
{
    public class IdempotentMessageService(
        AppDbContext dbContext,
        IAppLogger<IdempotentMessageService> logger) : IIdempotentMessageService
    {
        private static readonly ConcurrentDictionary<Guid, SemaphoreSlim> _locks = new();

        public async Task<bool> HasBeenProcessedAsync(Guid eventId, CancellationToken cancellationToken = default)
        {
            return await dbContext.ProcessedMessages
                .AsNoTracking()
                .AnyAsync(p => p.EventId == eventId, cancellationToken);
        }

        public async Task<bool> ExecuteIdempotentlyAsync(
            Guid eventId,
            string messageType,
            string consumerName,
            Func<Task> businessOperation,
            CancellationToken cancellationToken = default)
        {
            // 1. Initial quick check before executing operation
            if (await HasBeenProcessedAsync(eventId, cancellationToken))
            {
                logger.LogWarning($"[Idempotency] Duplicate message detected for EventId: {eventId}, MessageType: {messageType}, Consumer: {consumerName}. Ignoring duplicate message.");
                return false;
            }

            // Concurrency lock for concurrent deliveries of the same EventId
            var semaphore = _locks.GetOrAdd(eventId, _ => new SemaphoreSlim(1, 1));
            await semaphore.WaitAsync(cancellationToken);

            try
            {
                if (dbContext.Database.IsRelational())
                {
                    var strategy = dbContext.Database.CreateExecutionStrategy();
                    return await strategy.ExecuteAsync(async () =>
                    {
                        // 2. Double check under lock
                        if (await HasBeenProcessedAsync(eventId, cancellationToken))
                        {
                            logger.LogWarning($"[Idempotency] Duplicate message detected on retry for EventId: {eventId}. Ignoring duplicate message.");
                            return false;
                        }

                        // 3. Execute business operation (e.g. sending notification email)
                        // If this fails, exception is thrown, nothing is committed to ProcessedMessages.
                        await businessOperation();

                        // 4. Record EventId in database transaction
                        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
                        try
                        {
                            var alreadyRecorded = await dbContext.ProcessedMessages
                                .AnyAsync(p => p.EventId == eventId, cancellationToken);

                            if (alreadyRecorded)
                            {
                                logger.LogWarning($"[Idempotency] Concurrent duplicate delivery caught inside transaction for EventId: {eventId}. Rolling back.");
                                await transaction.RollbackAsync(cancellationToken);
                                return false;
                            }

                            var record = new ProcessedMessage
                            {
                                EventId = eventId,
                                MessageType = messageType,
                                ConsumerName = consumerName,
                                ProcessedAtUtc = DateTime.UtcNow
                            };

                            dbContext.ProcessedMessages.Add(record);
                            await dbContext.SaveChangesAsync(cancellationToken);
                            await transaction.CommitAsync(cancellationToken);

                            logger.LogInformation($"[Idempotency] EventId: {eventId} successfully processed and recorded by {consumerName}.");
                            return true;
                        }
                        catch (DbUpdateException ex)
                        {
                            await transaction.RollbackAsync(cancellationToken);
                            logger.LogWarning($"[Idempotency] Unique index constraint triggered for EventId: {eventId}. Duplicate delivery handled safely. Error: {ex.Message}");
                            return false;
                        }
                        catch (Exception)
                        {
                            await transaction.RollbackAsync(cancellationToken);
                            throw;
                        }
                    });
                }
                else
                {
                    // Non-relational (e.g. In-Memory test database)
                    if (await HasBeenProcessedAsync(eventId, cancellationToken))
                    {
                        return false;
                    }

                    await businessOperation();

                    if (await dbContext.ProcessedMessages.AnyAsync(p => p.EventId == eventId, cancellationToken))
                    {
                        return false;
                    }

                    dbContext.ProcessedMessages.Add(new ProcessedMessage
                    {
                        EventId = eventId,
                        MessageType = messageType,
                        ConsumerName = consumerName,
                        ProcessedAtUtc = DateTime.UtcNow
                    });
                    await dbContext.SaveChangesAsync(cancellationToken);

                    logger.LogInformation($"[Idempotency] EventId: {eventId} successfully processed and recorded by {consumerName}.");
                    return true;
                }
            }
            finally
            {
                semaphore.Release();
                _locks.TryRemove(eventId, out _);
            }
        }
    }
}
