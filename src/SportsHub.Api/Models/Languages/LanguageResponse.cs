namespace SportsHub.Api.Models.Languages;

/// <summary>
/// Response model for language data
/// </summary>
public class LanguageResponse
{
    /// <summary>
    /// Unique identifier for the language
    /// </summary>
    /// <example>1</example>
    public int Id { get; set; }

    /// <summary>
    /// Display name of the language
    /// </summary>
    /// <example>English</example>
    public string Name { get; set; }

    /// <summary>
    /// ISO language code
    /// </summary>
    /// <example>en</example>
    public string Code { get; set; }

    /// <summary>
    /// Whether the language is active
    /// </summary>
    /// <example>true</example>
    public bool IsActive { get; set; }

    /// <summary>
    /// Whether this language is English (protected)
    /// </summary>
    /// <example>true</example>
    public bool IsEnglish { get; set; }

    /// <summary>
    /// Whether this language can be deleted
    /// </summary>
    /// <example>false</example>
    public bool CanBeDeleted { get; set; }

    /// <summary>
    /// When the language was created
    /// </summary>
    /// <example>2024-01-01T00:00:00Z</example>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// When the language was last updated
    /// </summary>
    /// <example>2024-01-01T00:00:00Z</example>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// User ID who created the language
    /// </summary>
    /// <example>user123</example>
    public string CreatedByUserId { get; set; }

    /// <summary>
    /// User ID who last updated the language
    /// </summary>
    /// <example>user456</example>
    public string UpdatedByUserId { get; set; }
} 