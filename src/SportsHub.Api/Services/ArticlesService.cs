using SportsHub.Api.Models.Articles;
using SportsHub.Domain.Entities;
using SportsHub.Domain.Repositories;
using SportsHub.Infrastructure.Db;

namespace SportsHub.Api.Services;

/// <summary>
/// Service for managing articles in the Sports Hub application
/// </summary>
internal class ArticlesService : IArticlesService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IArticlesRepository _articlesRepository;
    private readonly IApplicationMapper _map;
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="ArticlesService"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work for database operations</param>
    /// <param name="articlesRepository">The repository for article data access</param>
    /// <param name="map">The mapper for converting between domain and API models</param>
    /// <param name="httpContextAccessor">The accessor for the current HTTP context</param>
    public ArticlesService(
        IUnitOfWork unitOfWork,
        IArticlesRepository articlesRepository,
        IApplicationMapper map,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _articlesRepository = articlesRepository;
        _map = map;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Creates a new article in the system
    /// </summary>
    /// <param name="request">The article creation data</param>
    /// <returns>The newly created article response</returns>
    public async Task<ArticleResponse> CreateArticle(CreateArticleRequest request)
    {
        var article = new Article(request.Title, request.ShortDescription, request.Description);

        _articlesRepository.Create(article);

        await _unitOfWork.CommitCurrentAsync();

        return _map.ToArticleResponse(article, GetBaseUrl());
    }

    /// <summary>
    /// Retrieves all articles from the system
    /// </summary>
    /// <returns>Array of article responses</returns>
    public async Task<ArticleResponse[]> GetArticles()
    {
        var articles = await _articlesRepository.GetAll();
        var baseUrl = GetBaseUrl();
        return articles.Select(a => _map.ToArticleResponse(a, baseUrl)).ToArray();
    }

    /// <summary>
    /// Retrieves a specific article by its ID
    /// </summary>
    /// <param name="articleId">The ID of the article to retrieve</param>
    /// <returns>The article response if found, null otherwise</returns>
    public async Task<ArticleResponse> GetArticle(int articleId)
    {
        var article = await _articlesRepository.GetById(articleId);

        return article is null ? null : _map.ToArticleResponse(article, GetBaseUrl());
    }

    /// <summary>
    /// Updates an existing article with new data
    /// </summary>
    /// <param name="articleId">The ID of the article to update</param>
    /// <param name="request">The article update data</param>
    /// <returns>The updated article response if found, null otherwise</returns>
    public async Task<ArticleResponse> UpdateArticle(int articleId, UpdateArticleRequest request)
    {
        var article = await _articlesRepository.GetById(articleId);
        if (article is null)
        {
            return null;
        }

        article.ApplyUpdate(request.Title, request.ShortDescription, request.Description);

        await _unitOfWork.CommitCurrentAsync();

        return _map.ToArticleResponse(article, GetBaseUrl());
    }

    /// <summary>
    /// Gets the base URL for the current HTTP request
    /// </summary>
    /// <returns>The base URL in the format scheme://host or null if HttpContext is not available</returns>
    private string GetBaseUrl()
    {
        var request = _httpContextAccessor?.HttpContext?.Request;
        return request is null
            ? null :
            $"{request.Scheme}://{request.Host}";
    }
}