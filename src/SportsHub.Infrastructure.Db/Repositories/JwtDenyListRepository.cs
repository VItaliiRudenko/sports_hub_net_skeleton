using Microsoft.EntityFrameworkCore;
using SportsHub.Domain.Entities;
using SportsHub.Domain.Repositories;

namespace SportsHub.Infrastructure.Db.Repositories;

internal class JwtDenyListRepository : RepositoryBase, IJwtDenyListRepository
{
    public JwtDenyListRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public void Create(JwtDenyRecord jwtDenyRecord)
    {
        DbContext.JwtDenyList.Add(jwtDenyRecord);
    }

    public Task<JwtDenyRecord> GetById(string jti)
    {
        return DbContext.JwtDenyList.AsNoTracking().FirstOrDefaultAsync(a => a.Jti == jti);
    }

    public Task DeleteExpired(DateTime maxExp)
    {
        return DbContext.JwtDenyList.Where(x => x.Exp < maxExp).ExecuteDeleteAsync();
    }
}