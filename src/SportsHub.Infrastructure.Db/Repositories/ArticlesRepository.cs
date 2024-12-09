using Microsoft.EntityFrameworkCore;
using SportsHub.Domain.Entities;
using SportsHub.Domain.Repositories;

namespace SportsHub.Infrastructure.Db.Repositories;

internal class ArticlesRepository : RepositoryBase, IArticlesRepository
{
    public ArticlesRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public void Create(Article article)
    {
        DbContext.Articles.Add(article);
    }

    public Task<List<Article>> GetAll()
    {
        return DbContext.Articles
            .Include(a => a.Comments)
            .AsNoTracking()
            .ToListAsync();
    }

    public Task<Article> GetById(int articleId)
    {
        return DbContext.Articles
            .Include(a => a.Comments)
            .FirstOrDefaultAsync(a => a.Id == articleId);
    }
}