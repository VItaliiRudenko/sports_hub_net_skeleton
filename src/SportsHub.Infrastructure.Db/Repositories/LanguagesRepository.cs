using Microsoft.EntityFrameworkCore;
using SportsHub.Domain.Entities;
using SportsHub.Domain.Repositories;

namespace SportsHub.Infrastructure.Db.Repositories;

internal class LanguagesRepository : RepositoryBase, ILanguagesRepository
{
    public LanguagesRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public Task<List<Language>> GetAll()
    {
        return DbContext.Languages
            .AsNoTracking()
            .OrderBy(l => l.Name)
            .ToListAsync();
    }

    public Task<Language> GetById(int languageId)
    {
        return DbContext.Languages
            .FirstOrDefaultAsync(l => l.Id == languageId);
    }

    public Task<Language> GetByCode(string code)
    {
        return DbContext.Languages
            .FirstOrDefaultAsync(l => l.Code == code);
    }

    public void Create(Language language)
    {
        DbContext.Languages.Add(language);
    }

    public void Update(Language language)
    {
        DbContext.Languages.Update(language);
    }

    public void Delete(Language language)
    {
        DbContext.Languages.Remove(language);
    }

    public Task<bool> ExistsByCode(string code)
    {
        return DbContext.Languages
            .AnyAsync(l => l.Code == code);
    }

    public Task<bool> ExistsByCodeExcludingId(string code, int excludeId)
    {
        return DbContext.Languages
            .AnyAsync(l => l.Code == code && l.Id != excludeId);
    }
} 