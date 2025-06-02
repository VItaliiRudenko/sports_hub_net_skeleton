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
    [Required]
    public SignInRequestUserModel User { get; set; }
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
    [Required]
    public string Email { get; set; }

    /// <summary>
    /// User's password
    /// </summary>
    /// <example>SecurePassword123</example>
    [Required]
    public string Password { get; set; }
}