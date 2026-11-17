namespace Application.Service.Contracts.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body, CancellationToken cancellationToken = default);
    }
}
