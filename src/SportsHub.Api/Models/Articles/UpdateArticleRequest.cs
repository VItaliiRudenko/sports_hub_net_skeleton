namespace SportsHub.Api.Models.Articles;

/// <summary>
/// Request model for updating an existing article
/// </summary>
public class UpdateArticleRequest
{
    /// <summary>
    /// Updated article title (optional)
    /// </summary>
    /// <example>Updated: Local Team Wins Championship</example>
    public string Title { get; set; }
    
    /// <summary>
    /// Updated brief summary of the article (optional)
    /// </summary>
    /// <example>The local basketball team secured their first championship victory in overtime...</example>
    public string ShortDescription { get; set; }
    
    /// <summary>
    /// Updated full article content (optional)
    /// </summary>
    /// <example>In an exciting match that went into overtime, the local basketball team demonstrated...</example>
    public string Description { get; set; }
}