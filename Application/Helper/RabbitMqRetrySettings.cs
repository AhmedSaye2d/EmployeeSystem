namespace Application.Helper
{
    public class RabbitMqRetrySettings
    {
        public int MaxRetryAttempts { get; set; } = 3;
        public int InitialIntervalSeconds { get; set; } = 2;
        public int IntervalIncrementSeconds { get; set; } = 3;
        public string VacationApprovedQueue { get; set; } = "vacation-approved";
        public string VacationRejectedQueue { get; set; } = "vacation-rejected";
        public string DeadLetterExchange { get; set; } = "vacation-dead-letter-exchange";
        public string DeadLetterQueue { get; set; } = "vacation-dead-letter-queue";
    }
}
