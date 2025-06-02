namespace SportsHub.Domain.Entities;

/// <summary>
/// Represents an article in the Sports Hub system
/// </summary>
public class Article : AuditEntity
{
    /// <summary>
    /// Gets the title of the article
    /// </summary>
    public string Title { get; private set; }
    /// <summary>
    /// Gets the short description of the article
    /// </summary>
    public string ShortDescription { get; private set; }
    /// <summary>
    /// Gets the full description/content of the article
    /// </summary>
    public string Description { get; private set; }
    /// <summary>
    /// Gets the filename of the article's main image
    /// </summary>
    public string ImageFileName { get; private set; }
    /// <summary>
    /// Gets the number of likes for the article
    /// </summary>
    public int ArticleLikes { get; private set; }
    /// <summary>
    /// Gets the number of dislikes for the article
    /// </summary>
    public int ArticleDislikes { get; private set; }
    /// <summary>
    /// Gets the list of comments associated with this article
    /// </summary>
    public List<ArticleComment> Comments { get; init; } = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Article"/> class
    /// </summary>
    /// <param name="title">The title of the article</param>
    /// <param name="shortDescription">The short description of the article</param>
    /// <param name="description">The full description/content of the article</param>
    public Article(string title, string shortDescription, string description)
    {
        Title = title;
        ShortDescription = shortDescription;
        Description = description;
    }

    /// <summary>
    /// Updates the article with new data if provided
    /// </summary>
    /// <param name="title">The new title (if null or empty, the existing title is retained)</param>
    /// <param name="shortDescription">The new short description (if null or empty, the existing short description is retained)</param>
    /// <param name="description">The new full description/content (if null or empty, the existing description is retained)</param>
    public void ApplyUpdate(string title, string shortDescription, string description)
    {
        if (!string.IsNullOrWhiteSpace(title))
        {
            Title = title;
        }

        if (!string.IsNullOrWhiteSpace(shortDescription))
        {
            ShortDescription = shortDescription;
        }

        if (!string.IsNullOrWhiteSpace(description))
        {
            Description = description;
        }
    }

    /// <summary>
    /// Sets the filename of the article's main image
    /// </summary>
    /// <param name="imageFileName">The filename of the image</param>
    public void SetImage(string imageFileName)
    {
        ImageFileName = imageFileName;
    }

    /// <summary>
    /// Updates the reaction counts (likes and dislikes) for the article
    /// </summary>
    /// <param name="likes">The new number of likes</param>
    /// <param name="dislikes">The new number of dislikes</param>
    public void UpdateReactionsData(int likes, int dislikes)
    {
        ArticleLikes = likes;
        ArticleDislikes = dislikes;
    }
}