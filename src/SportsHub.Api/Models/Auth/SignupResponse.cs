namespace SportsHub.Api.Models.Auth;

/// <summary>
/// Response model for successful user registration
/// </summary>
public class SignupResponse
{
    /// <summary>
    /// Unique user identifier
    /// </summary>
    /// <example>12345</example>
    public string Id { get; set; }
    
    /// <summary>
    /// User's registered email address
    /// </summary>
    /// <example>user@example.com</example>
    public string Email { get; set; }
}