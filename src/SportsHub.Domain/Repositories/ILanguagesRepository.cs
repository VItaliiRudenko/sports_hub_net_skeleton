using SportsHub.Domain.Entities;

namespace SportsHub.Domain.Repositories;

public interface ILanguagesRepository
{
    Task<List<Language>> GetAll();
    Task<Language> GetById(int languageId);
    Task<Language> GetByCode(string code);
    void Create(Language language);
    void Update(Language language);
    void Delete(Language language);
    Task<bool> ExistsByCode(string code);
    Task<bool> ExistsByCodeExcludingId(string code, int excludeId);
} 