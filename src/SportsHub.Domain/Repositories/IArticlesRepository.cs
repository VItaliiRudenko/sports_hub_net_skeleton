using SportsHub.Domain.Entities;

namespace SportsHub.Domain.Repositories;

public interface IArticlesRepository
{
    void Create(Article article);
    Task<List<Article>> GetAll();
    Task<Article> GetById(int articleId);
}