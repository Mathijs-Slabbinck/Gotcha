using System.Net.Mail;
using Gotcha.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Gotcha.Core.Services.Email
{
    // Sends emails via SMTP. Reads connection settings from appsettings.json ("EmailSettings" section).
    // Dev: uses MailHog (localhost:1025, no SSL, no credentials)
    // Prod: swap config to SendGrid or another provider
    public class EmailService : IEmailService
    {
        // SMTP connection settings (loaded from appsettings.json)
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _fromEmail;
        private readonly string _fromName;

        // Reads EmailSettings from appsettings.json via IConfiguration (injected by DI)
        public EmailService(IConfiguration configuration)
        {
            _smtpHost = configuration["EmailSettings:SmtpHost"]!;
            _smtpPort = int.Parse(configuration["EmailSettings:SmtpPort"]!);
            _fromEmail = configuration["EmailSettings:FromEmail"]!;
            _fromName = configuration["EmailSettings:FromName"]!;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            // Build the email message
            MailMessage message = new MailMessage
            {
                From = new MailAddress(_fromEmail, _fromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true, // allows HTML in the body (for styled confirmation emails)
            };

            message.To.Add(toEmail);

            // Create the SMTP client and send
            // "using" ensures the client is disposed after sending (frees the connection)
            // EnableSsl = false because MailHog doesn't use SSL (switch to true for production)
            using SmtpClient client = new SmtpClient(_smtpHost, _smtpPort)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = false,
            };

            await client.SendMailAsync(message);
        }
    }
}
