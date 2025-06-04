namespace SportsHub.Api.Models.Articles;

/// <summary>
/// Response model for article data
/// </summary>
public class ArticleResponse
{
    /// <summary>
    /// Unique article identifier
    /// </summary>
    /// <example>1</example>
    public int Id { get; set; }
    
    /// <summary>
    /// Article title
    /// </summary>
    /// <example>Breaking: Local Team Wins Championship</example>
    public string Title { get; set; }
    
    /// <summary>
    /// Brief summary of the article
    /// </summary>
    /// <example>The local basketball team secured their first championship victory...</example>
    public string ShortDescription { get; set; }
    
    /// <summary>
    /// Full article content
    /// </summary>
    /// <example>In an exciting match that went into overtime, the local basketball team...</example>
    public string Description { get; set; }
    
    /// <summary>
    /// URL to the article's featured image
    /// </summary>
    /// <example>https://example.com/images/article-1.jpg</example>
    public string ImageUrl { get; set; }
    
    /// <summary>
    /// Number of likes for this article
    /// </summary>
    /// <example>42</example>
    public int ArticleLikes { get; set; }
    
    /// <summary>
    /// Number of dislikes for this article
    /// </summary>
    /// <example>3</example>
    public int ArticleDislikes { get; set; }
    
    /// <summary>
    /// List of comment contents for this article
    /// </summary>
    /// <example>["Great article!", "Thanks for sharing"]</example>
    public List<string> CommentsContent { get; set; } = new();
    
    /// <summary>
    /// Total number of comments on this article
    /// </summary>
    /// <example>15</example>
    public int CommentsCount { get; set; }
    
    /// <summary>
    /// Date and time when the article was created
    /// </summary>
    /// <example>2024-01-15T10:30:00Z</example>
    public DateTimeOffset CreatedAt { get; set; }
    
    /// <summary>
    /// Date and time when the article was last updated (null if never updated)
    /// </summary>
    /// <example>2024-01-16T14:20:00Z</example>
    public DateTimeOffset? UpdatedAt { get; set; }
}