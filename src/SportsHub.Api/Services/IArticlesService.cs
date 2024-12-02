using SportsHub.Api.Models.Articles;

namespace SportsHub.Api.Services;

public interface IArticlesService
{
    Task<ArticleResponse> CreateArticle(CreateArticleRequest request);
    Task<ArticleResponse[]> GetArticles();
    Task<ArticleResponse> GetArticle(int articleId);
    Task<ArticleResponse> UpdateArticle(int articleId, UpdateArticleRequest request);
}