namespace SportsHub.Api.Models.Configuration;

/// <summary>
/// Configuration settings for CORS
/// </summary>
public class CorsSettings
{
    public const string SectionName = "Cors";

    /// <summary>
    /// Allowed origins for CORS requests (semicolon-separated)
    /// </summary>
    public string AllowedOrigins { get; set; } = string.Empty;

    /// <summary>
    /// Gets the allowed origins as an array
    /// </summary>
    public string[] GetAllowedOriginsArray()
    {
        return AllowedOrigins.Split(';', StringSplitOptions.RemoveEmptyEntries);
    }
} 