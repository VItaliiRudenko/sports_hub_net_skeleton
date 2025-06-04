namespace SportsHub.Api.Models.Auth;

/// <summary>
/// Response model for successful user authentication
/// </summary>
public class SignInResponse
{
    /// <summary>
    /// Unique user identifier
    /// </summary>
    /// <example>12345</example>
    public string Id { get; set; }
    
    /// <summary>
    /// User's email address
    /// </summary>
    /// <example>user@example.com</example>
    public string Email { get; set; }
    
    /// <summary>
    /// JWT authentication token for API access
    /// </summary>
    /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
    public string AuthenticationToken { get; set; }
}