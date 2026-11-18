namespace Application.Service.Contracts.Idempotency
{
    public interface IIdempotentMessageService
    {
        Task<bool> HasBeenProcessedAsync(Guid eventId, CancellationToken cancellationToken = default);

        Task<bool> ExecuteIdempotentlyAsync(
            Guid eventId,
            string messageType,
            string consumerName,
            Func<Task> businessOperation,
            CancellationToken cancellationToken = default);
    }
}
