using System.ComponentModel.DataAnnotations;

namespace SportsHub.Api.Models.Auth;

/// <summary>
/// Request model for user sign in
/// </summary>
public class SignInRequest
{
    /// <summary>
    /// User authentication credentials
    /// </summary>
    [Required(ErrorMessage = "User credentials are required")]
    public SignInRequestUserModel User { get; set; } = new();
}

/// <summary>
/// User credentials for authentication
/// </summary>
public class SignInRequestUserModel
{
    /// <summary>
    /// User's email address
    /// </summary>
    /// <example>user@example.com</example>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please provide a valid email address")]
    [StringLength(254, ErrorMessage = "Email cannot exceed 254 characters")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// User's password
    /// </summary>
    /// <example>SecurePassword123!</example>
    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;
}