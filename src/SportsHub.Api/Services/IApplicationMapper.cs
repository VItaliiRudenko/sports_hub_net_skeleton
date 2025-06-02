using SportsHub.Api.Models.Articles;
using SportsHub.Domain.Entities;

namespace SportsHub.Api.Services;

/// <summary>
/// Interface for mapping domain entities to API response models.
/// </summary>
public interface IApplicationMapper
{
    /// <summary>
    /// Converts an Article domain entity to an ArticleResponse API model.
    /// </summary>
    /// <param name="article">The Article domain entity to convert.</param>
    /// <param name="apiBaseUrl">The base URL for constructing resource URLs.</param>
    /// <returns>An ArticleResponse containing the mapped article data.</returns>
    ArticleResponse ToArticleResponse(Article article, string apiBaseUrl);
}