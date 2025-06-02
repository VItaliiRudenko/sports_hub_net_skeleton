using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace SportsHub.Api.Services;

/// <summary>
/// Service responsible for sending emails, particularly password reset emails.
/// Skipps emails when SMTP configuration is missing.
/// </summary>
public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    /// <summary>
    /// Initializes a new instance of the EmailService.
    /// </summary>
    /// <param name="configuration">Configuration provider for SMTP settings. Can be null for graceful degradation.</param>
    /// <param name="logger">Logger instance for logging email operations and errors.</param>
    /// <exception cref="ArgumentNullException">Thrown when logger is null.</exception>
    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration ?? new ConfigurationBuilder().Build(); // Use empty configuration if null
        _logger = logger ?? throw new ArgumentNullException(nameof(logger)); // Logger is still required for proper error handling
    }

    /// <summary>
    /// Sends a password reset email to the specified email address with a reset token.
    /// If SMTP configuration is missing or incomplete, the operation will be gracefully skipped with a warning log.
    /// </summary>
    /// <param name="email">The recipient's email address.</param>
    /// <param name="resetToken">The password reset token to include in the email.</param>
    /// <returns>A task representing the asynchronous email sending operation.</returns>
    /// <exception cref="Exception">Thrown when email sending fails due to SMTP errors.</exception>
    /// <remarks>
    /// Required SMTP configuration keys:
    /// - Smtp:Host: SMTP server hostname
    /// - Smtp:Port: SMTP server port (must be a valid integer)
    /// - Smtp:Username: SMTP authentication username
    /// - Smtp:Password: SMTP authentication password
    /// 
    /// The email will contain a reset link pointing to http://localhost:3000/reset-password?token={resetToken}
    /// </remarks>
    public async Task SendPasswordResetEmailAsync(string email, string resetToken)
    {
        try
        {
            var smtpHost = _configuration["Smtp:Host"];
            var smtpPortString = _configuration["Smtp:Port"];
            var smtpUsername = _configuration["Smtp:Username"];
            var smtpPassword = _configuration["Smtp:Password"];

            // Check if SMTP configuration is available
            if (string.IsNullOrEmpty(smtpHost) || 
                string.IsNullOrEmpty(smtpPortString) || 
                !int.TryParse(smtpPortString, out var smtpPort) ||
                string.IsNullOrEmpty(smtpUsername) || 
                string.IsNullOrEmpty(smtpPassword))
            {
                _logger.LogWarning("SMTP configuration is missing or incomplete. Email sending is disabled.");
                return; // Gracefully skip sending email
            }

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                EnableSsl = true,
                Credentials = new System.Net.NetworkCredential(smtpUsername, smtpPassword)
            };

            var resetLink = $"http://localhost:3000/reset-password?token={resetToken}";
            var message = new MailMessage
            {
                From = new MailAddress(smtpUsername), // Use configured username as sender
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
            _logger.LogInformation("Password reset email sent successfully to {Email}", email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send password reset email to {Email}", email);
            throw;
        }
    }
} 