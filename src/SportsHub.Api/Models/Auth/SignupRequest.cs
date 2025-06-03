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
    [Required(ErrorMessage = "Registration details are required")]
    public SignupRequestUserModel Registration { get; set; } = new();
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
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please provide a valid email address")]
    [StringLength(254, ErrorMessage = "Email cannot exceed 254 characters")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// User's password (minimum 8 characters with complexity requirements)
    /// </summary>
    /// <example>SecurePassword123!</example>
    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]", 
        ErrorMessage = "Password must contain at least one lowercase letter, one uppercase letter, one number, and one special character")]
    public string Password { get; set; } = string.Empty;
    
    /// <summary>
    /// Password confirmation (must match password)
    /// </summary>
    /// <example>SecurePassword123!</example>
    [Required(ErrorMessage = "Password confirmation is required")]
    [Compare(nameof(Password), ErrorMessage = "Password and confirmation do not match")]
    public string PasswordConfirmation { get; set; } = string.Empty;
}

