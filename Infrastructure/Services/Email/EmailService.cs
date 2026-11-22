using Application.Helper;
using Application.Service.Contracts.Email;
using Application.Service.Contracts.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Services.Email
{
    /// <summary>
    /// Concrete Email Service implementation.
    /// Handles sending emails via SMTP, with structured logging via IAppLogger.
    /// In local development or when credentials are not configured, gracefully logs the email.
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly IAppLogger<EmailService> _logger;

        public EmailService(
            IOptions<EmailSettings> settings,
            IAppLogger<EmailService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(
            string toEmail,
            string subject,
            string body,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(toEmail))
            {
                _logger.LogWarning("Email sending skipped: Recipient email address is empty.");
                return;
            }

            try
            {
                // If SMTP host is configured for real delivery
                if (!string.IsNullOrWhiteSpace(_settings.SmtpHost) && _settings.SmtpHost != "localhost")
                {
                    using var client = new SmtpClient(_settings.SmtpHost, _settings.SmtpPort)
                    {
                        EnableSsl = _settings.EnableSsl,
                        Credentials = !string.IsNullOrWhiteSpace(_settings.Username)
                            ? new NetworkCredential(_settings.Username, _settings.Password)
                            : CredentialCache.DefaultNetworkCredentials
                    };

                    using var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_settings.FromEmail, _settings.FromName),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };
                    mailMessage.To.Add(toEmail);

                    await client.SendMailAsync(mailMessage, cancellationToken);
                    _logger.LogInformation($"Email successfully dispatched to {toEmail} with subject '{subject}'.");
                }
                else
                {
                    // Simulated / Development email delivery (Logged for audit and debugging)
                    _logger.LogInformation(
                        $"[DEV EMAIL DISPATCHED] To: {toEmail} | Subject: {subject} | Content: {body.Replace(Environment.NewLine, " ")}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send email to {toEmail} with subject '{subject}'.");
                throw; // Rethrow to allow MassTransit Consumer retry mechanism to handle transient errors
            }
        }
    }
}
