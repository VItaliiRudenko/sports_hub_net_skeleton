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
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please provide a valid email address")]
    [StringLength(254, ErrorMessage = "Email cannot exceed 254 characters")]
    public string Email { get; set; } = string.Empty;
} 