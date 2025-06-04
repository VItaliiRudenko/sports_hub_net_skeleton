namespace SportsHub.Domain.Entities;

/// <summary>
/// Represents a language in the system with localization support
/// </summary>
public class Language : AuditEntity
{
    /// <summary>
    /// Display name of the language (e.g., "English", "Spanish")
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// ISO language code (e.g., "en", "es", "fr")
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Indicates whether the language is active and available for use
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Checks if this language is English (protected language that cannot be deleted)
    /// </summary>
    public bool IsEnglish => Code?.ToLowerInvariant() == "en";

    /// <summary>
    /// Determines if this language can be deleted (all languages except English)
    /// </summary>
    public bool CanBeDeleted => !IsEnglish;
} 