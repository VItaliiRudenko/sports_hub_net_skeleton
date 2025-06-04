using SportsHub.Api.Models.Languages;
using SportsHub.Domain.Entities;
using SportsHub.Domain.Repositories;
using SportsHub.Infrastructure.Db;

namespace SportsHub.Api.Services;

/// <summary>
/// Service responsible for language management operations
/// </summary>
public class LanguagesService : ILanguagesService
{
    private readonly ILanguagesRepository _languagesRepository;
    private readonly IApplicationMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<LanguagesService> _logger;

    public LanguagesService(
        ILanguagesRepository languagesRepository,
        IApplicationMapper mapper,
        IUnitOfWork unitOfWork,
        ILogger<LanguagesService> logger)
    {
        _languagesRepository = languagesRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<LanguageResponse[]> GetLanguages()
    {
        var languages = await _languagesRepository.GetAll();
        return languages.Select(_mapper.ToLanguageResponse).ToArray();
    }

    public async Task<LanguageResponse> GetLanguage(int languageId)
    {
        var language = await _languagesRepository.GetById(languageId);
        return language == null ? null : _mapper.ToLanguageResponse(language);
    }

    public async Task<LanguageResponse> CreateLanguage(CreateLanguageRequest request)
    {
        // Validate that language code doesn't already exist
        var existingLanguage = await _languagesRepository.ExistsByCode(request.Code);
        if (existingLanguage)
        {
            throw new InvalidOperationException($"Language with code '{request.Code}' already exists.");
        }

        var language = new Language
        {
            Name = request.Name,
            Code = request.Code.ToLowerInvariant(),
            IsActive = request.IsActive
        };

        _languagesRepository.Create(language);
        await _unitOfWork.CommitCurrentAsync();

        _logger.LogInformation("Created new language: {LanguageName} ({LanguageCode})", language.Name, language.Code);

        return _mapper.ToLanguageResponse(language);
    }

    public async Task<LanguageResponse> UpdateLanguage(int languageId, UpdateLanguageRequest request)
    {
        var language = await _languagesRepository.GetById(languageId);
        if (language == null)
        {
            return null;
        }

        // Validate that language code doesn't already exist for another language
        var codeExists = await _languagesRepository.ExistsByCodeExcludingId(request.Code, languageId);
        if (codeExists)
        {
            throw new InvalidOperationException($"Language with code '{request.Code}' already exists.");
        }

        language.Name = request.Name;
        language.Code = request.Code.ToLowerInvariant();
        language.IsActive = request.IsActive;

        _languagesRepository.Update(language);
        await _unitOfWork.CommitCurrentAsync();

        _logger.LogInformation("Updated language: {LanguageName} ({LanguageCode})", language.Name, language.Code);

        return _mapper.ToLanguageResponse(language);
    }

    public async Task<bool> DeleteLanguage(int languageId)
    {
        var language = await _languagesRepository.GetById(languageId);
        if (language == null)
        {
            return false;
        }

        // Prevent deletion of English language
        if (language.IsEnglish)
        {
            throw new InvalidOperationException("English language cannot be deleted as it is a protected language.");
        }

        _languagesRepository.Delete(language);
        await _unitOfWork.CommitCurrentAsync();

        _logger.LogInformation("Deleted language: {LanguageName} ({LanguageCode})", language.Name, language.Code);

        return true;
    }
} 