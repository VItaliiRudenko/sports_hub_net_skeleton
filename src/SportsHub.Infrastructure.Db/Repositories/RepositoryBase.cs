namespace SportsHub.Infrastructure.Db.Repositories;

public abstract class RepositoryBase
{
    protected readonly AppDbContext DbContext;

    internal RepositoryBase(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }
}
