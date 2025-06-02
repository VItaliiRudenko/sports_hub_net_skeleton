using System.ComponentModel.DataAnnotations;

namespace SportsHub.Api.Models.Auth;

/// <summary>
/// Request model for resetting user password
/// </summary>
public class ResetPasswordRequest
{
    /// <summary>
    /// Password reset token received via email
    /// </summary>
    /// <example>abc123def456ghi789</example>
    [Required]
    public string Token { get; set; }

    /// <summary>
    /// User's email address
    /// </summary>
    /// <example>user@example.com</example>
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    /// <summary>
    /// New password for the user account
    /// </summary>
    /// <example>NewSecurePassword123</example>
    [Required]
    public string Password { get; set; }

    /// <summary>
    /// Password confirmation (must match password)
    /// </summary>
    /// <example>NewSecurePassword123</example>
    [Required]
    public string PasswordConfirmation { get; set; }
} 