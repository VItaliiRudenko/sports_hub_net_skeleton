using SportsHub.Domain.Entities;

namespace SportsHub.Api.Services;

public interface IArticlesService
{
    Task<Article> CreateArticle();
}