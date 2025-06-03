namespace SportsHub.Api.Models.Configuration;

/// <summary>
/// Configuration settings for JWT authentication
/// </summary>
public class JwtSettings
{
    public const string SectionName = "Jwt";

    /// <summary>
    /// JWT token issuer
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// JWT token audience
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// JWT token expiration time in minutes
    /// </summary>
    public int ExpirationMinutes { get; set; } = 60;

    /// <summary>
    /// Secret key for signing JWT tokens
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;
} 