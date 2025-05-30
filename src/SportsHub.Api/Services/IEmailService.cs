namespace SportsHub.Api.Services;

public interface IEmailService
{
    Task SendPasswordResetEmailAsync(string email, string resetToken);
} 