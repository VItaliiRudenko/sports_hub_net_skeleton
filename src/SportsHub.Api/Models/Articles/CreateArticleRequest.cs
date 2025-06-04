using System.ComponentModel.DataAnnotations;

namespace SportsHub.Api.Models.Articles;

/// <summary>
/// Request model for creating a new article
/// </summary>
public class CreateArticleRequest
{
    /// <summary>
    /// Article title
    /// </summary>
    /// <example>Breaking: Local Team Wins Championship</example>
    [Required]
    public string Title { get; set; }

    /// <summary>
    /// Brief summary of the article
    /// </summary>
    /// <example>The local basketball team secured their first championship victory...</example>
    [Required]
    public string ShortDescription { get; set; }

    /// <summary>
    /// Full article content
    /// </summary>
    /// <example>In an exciting match that went into overtime, the local basketball team...</example>
    [Required]
    public string Description { get; set; }
}