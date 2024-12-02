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

    public ArticlesService(
        IUnitOfWork unitOfWork,
        IArticlesRepository articlesRepository,
        IApplicationMapper map)
    {
        _unitOfWork = unitOfWork;
        _articlesRepository = articlesRepository;
        _map = map;
    }

    public async Task<ArticleResponse> CreateArticle()
    {
        var article = new Article
        {
            Title = "The title",
            ShortDescription = "Short desc",
            Description = "Asdaf asdijdfab asdkjmvdadg as dgojkdb"
        };

        _articlesRepository.Create(article);

        await _unitOfWork.CommitCurrentAsync();

        return _map.ToArticleResponse(article);
    }

    public async Task<ArticleResponse[]> GetArticles()
    {
        var articles = await _articlesRepository.GetAll();
        return articles.Select(a => _map.ToArticleResponse(a)).ToArray();
    }

    public async Task<ArticleResponse> UpdateArticle()
    {
        throw new NotImplementedException();
    }
}