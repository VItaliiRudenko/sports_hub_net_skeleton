using SportsHub.Api.Models.Articles;
using SportsHub.Domain.Entities;

namespace SportsHub.Api.Services;

/// <summary>
/// Service responsible for mapping domain entities to API response models.
/// Handles conversion between internal domain objects and external API representations.
/// </summary>
internal class ApplicationMapper : IApplicationMapper
{
    /// <summary>
    /// Converts an Article domain entity to an ArticleResponse API model.
    /// </summary>
    /// <param name="article">The Article domain entity to convert.</param>
    /// <param name="apiBaseUrl">The base URL for constructing image URLs. Can be null or empty.</param>
    /// <returns>An ArticleResponse containing the mapped article data with properly formatted image URLs.</returns>
    /// <remarks>
    /// - Converts DateTime properties to DateTimeOffset for API consistency
    /// - Generates image URLs using the pattern: {apiBaseUrl}/api/article-images/{imageFileName}
    /// - If apiBaseUrl is null or empty, uses relative URLs starting with "/"
    /// - Maps article comments to a simplified string list representation
    /// </remarks>
    public ArticleResponse ToArticleResponse(Article article, string apiBaseUrl)
    {
        return new ArticleResponse
        {
            Id = article.Id,
            Title = article.Title,
            ShortDescription = article.ShortDescription,
            Description = article.Description,
            ArticleLikes = article.ArticleLikes,
            ArticleDislikes = article.ArticleDislikes,
            CommentsCount = article.Comments.Count,
            CommentsContent = article.Comments.Select(c => c.CommentText).ToList(),
            ImageUrl = $"{apiBaseUrl}/api/article-images/{article.ImageFileName}",
            CreatedAt = article.CreatedAt,
            UpdatedAt = article.UpdatedAt,
        };
    }
}