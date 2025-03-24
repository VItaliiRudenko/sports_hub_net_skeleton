using SportsHub.Api.Models.Articles;
using SportsHub.Domain.Entities;
using SportsHub.Domain.Repositories;
using SportsHub.Infrastructure.Db;

namespace SportsHub.Api.Services;

internal class ArticlesService : IArticlesService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IArticlesRepository _articlesRepository;
    private readonly IApplicationMapper _map;
    private readonly IHttpContextAccessor _httpContextAccessor;

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

    public async Task<ArticleResponse> CreateArticle(CreateArticleRequest request)
    {
        var article = new Article(request.Title, request.ShortDescription, request.Description);

        _articlesRepository.Create(article);

        await _unitOfWork.CommitCurrentAsync();

        return _map.ToArticleResponse(article, GetBaseUrl());
    }

    public async Task<ArticleResponse[]> GetArticles()
    {
        var articles = await _articlesRepository.GetAll();
        var baseUrl = GetBaseUrl();
        return articles.Select(a => _map.ToArticleResponse(a, baseUrl)).ToArray();
    }

    public async Task<ArticleResponse> GetArticle(int articleId)
    {
        var article = await _articlesRepository.GetById(articleId);

        return article is null ? null : _map.ToArticleResponse(article, GetBaseUrl());
    }

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

    private string GetBaseUrl()
    {
        var request = _httpContextAccessor?.HttpContext?.Request;
        return request is null
            ? null :
            $"{request.Scheme}://{request.Host}";
    }
}