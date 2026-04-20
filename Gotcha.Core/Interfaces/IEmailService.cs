namespace Gotcha.Core.Interfaces
{
    // Interface for sending emails (signup confirmation, guardian consent, password reset)
    // Implementation: Gotcha.Core/Services/Email/EmailService.cs
    public interface IEmailService
    {
        // Sends an HTML email to the given address
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
