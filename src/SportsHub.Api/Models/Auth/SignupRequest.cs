using System.ComponentModel.DataAnnotations;

namespace SportsHub.Api.Models.Auth;

/// <summary>
/// Request model for user registration
/// </summary>
public class SignupRequest
{
    /// <summary>
    /// User registration details
    /// </summary>
    [Required]
    public SignupRequestUserModel Registration { get; set; }
}

/// <summary>
/// User details for registration
/// </summary>
public class SignupRequestUserModel
{
    /// <summary>
    /// User's email address
    /// </summary>
    /// <example>user@example.com</example>
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    /// <summary>
    /// User's password (minimum 6 characters)
    /// </summary>
    /// <example>SecurePassword123</example>
    [Required]
    public string Password { get; set; }
    
    /// <summary>
    /// Password confirmation (must match password)
    /// </summary>
    /// <example>SecurePassword123</example>
    [Required]
    public string PasswordConfirmation { get; set; }
}

