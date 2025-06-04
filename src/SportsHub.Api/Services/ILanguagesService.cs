using SportsHub.Api.Models.Languages;

namespace SportsHub.Api.Services;

public interface ILanguagesService
{
    Task<LanguageResponse[]> GetLanguages();
    Task<LanguageResponse> GetLanguage(int languageId);
    Task<LanguageResponse> CreateLanguage(CreateLanguageRequest request);
    Task<LanguageResponse> UpdateLanguage(int languageId, UpdateLanguageRequest request);
    Task<bool> DeleteLanguage(int languageId);
} 