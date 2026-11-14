namespace Domain.Entities
{
    public class ProcessedMessage
    {
        public int Id { get; set; }
        public Guid EventId { get; set; }
        public string MessageType { get; set; } = string.Empty;
        public string ConsumerName { get; set; } = string.Empty;
        public DateTime ProcessedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
