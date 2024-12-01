using SportsHub.Domain.Entities;
using SportsHub.Infrastructure.Db;

namespace SportsHub.Api.Services;

internal class ArticlesService : IArticlesService
{
    private readonly AppDbContext _db;

    public ArticlesService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Article> CreateArticle()
    {
        var res = new Article
        {
            Title = "The title",
            ShortDescription = "Short desc",
            Description = "Asdaf asdijdfab asdkjmvdadg as dgojkdb"
        };

        _db.Articles.Add(res);
        await _db.SaveChangesAsync();

        return res;
    }
}