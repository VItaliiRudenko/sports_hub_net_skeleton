using SportsHub.Api.Models.Articles;

namespace SportsHub.Api.Services;

/// <summary>
/// Defines the contract for article management services
/// </summary>
public interface IArticlesService
{
    /// <summary>
    /// Creates a new article
    /// </summary>
    /// <param name="request">The article creation data</param>
    /// <returns>The newly created article response</returns>
    Task<ArticleResponse> CreateArticle(CreateArticleRequest request);
    /// <summary>
    /// Retrieves all articles
    /// </summary>
    /// <returns>Array of article responses</returns>
    Task<ArticleResponse[]> GetArticles();
    /// <summary>
    /// Retrieves a specific article by its ID
    /// </summary>
    /// <param name="articleId">The ID of the article to retrieve</param>
    /// <returns>The article response if found, null otherwise</returns>
    Task<ArticleResponse> GetArticle(int articleId);
    /// <summary>
    /// Updates an existing article with new data
    /// </summary>
    /// <param name="articleId">The ID of the article to update</param>
    /// <param name="request">The article update data</param>
    /// <returns>The updated article response if found, null otherwise</returns>
    Task<ArticleResponse> UpdateArticle(int articleId, UpdateArticleRequest request);
}