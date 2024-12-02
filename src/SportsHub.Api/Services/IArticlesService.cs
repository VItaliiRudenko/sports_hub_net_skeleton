using SportsHub.Api.Models.Articles;

namespace SportsHub.Api.Services;

public interface IArticlesService
{
    Task<ArticleResponse> CreateArticle();
    Task<ArticleResponse[]> GetArticles();
    Task<ArticleResponse> UpdateArticle();
}