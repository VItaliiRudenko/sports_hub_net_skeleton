using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace SportsHub.Api.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendPasswordResetEmailAsync(string email, string resetToken)
    {
        try
        {
            using var client = new SmtpClient(_configuration["Smtp:Host"], int.Parse(_configuration["Smtp:Port"]))
            {
                EnableSsl = true,
                Credentials = new System.Net.NetworkCredential(_configuration["Smtp:Username"], _configuration["Smtp:Password"])
            };

            var resetLink = $"http://localhost:3000/reset-password?token={resetToken}";
            var message = new MailMessage
            {
                From = new MailAddress("noreply@gmail.com"),
                Subject = "Reset Your Password",
                Body = $@"
                    <h2>Password Reset Request</h2>
                    <p>You have requested to reset your password. Click the link below to proceed:</p>
                    <p><a href='{resetLink}'>{resetLink}</a></p>
                    <p>This link will expire in 15 minutes.</p>
                    <p>If you did not request this password reset, please ignore this email.</p>",
                IsBodyHtml = true
            };
            message.To.Add(email);

            await client.SendMailAsync(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send password reset email to {Email}", email);
            throw;
        }
    }
} 