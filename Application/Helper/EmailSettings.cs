namespace Application.Helper
{
    public class EmailSettings
    {
        public string FromEmail { get; set; } = "noreply@employeesystem.com";
        public string FromName { get; set; } = "Employee Management System";
        public string SmtpHost { get; set; } = "localhost";
        public int SmtpPort { get; set; } = 25;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool EnableSsl { get; set; } = false;
    }
}
