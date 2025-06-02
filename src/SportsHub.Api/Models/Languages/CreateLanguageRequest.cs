using System.ComponentModel.DataAnnotations;

namespace SportsHub.Api.Models.Languages;

/// <summary>
/// Request model for creating a new language
/// </summary>
public class CreateLanguageRequest
{
    /// <summary>
    /// Display name of the language
    /// </summary>
    /// <example>English</example>
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; set; }

    /// <summary>
    /// ISO language code
    /// </summary>
    /// <example>en</example>
    [Required]
    [StringLength(10, MinimumLength = 1)]
    public string Code { get; set; }

    /// <summary>
    /// Whether the language is active
    /// </summary>
    /// <example>true</example>
    public bool IsActive { get; set; } = true;
} 