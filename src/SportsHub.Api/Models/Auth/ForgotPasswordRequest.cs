using System.ComponentModel.DataAnnotations;

namespace SportsHub.Api.Models.Auth;

/// <summary>
/// Request model for password reset
/// </summary>
public class ForgotPasswordRequest
{
    /// <summary>
    /// User's email address for password reset
    /// </summary>
    /// <example>user@example.com</example>
    [Required]
    [EmailAddress]
    public string Email { get; set; }
} 