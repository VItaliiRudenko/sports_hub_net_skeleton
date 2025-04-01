using FakeItEasy;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using SportsHub.Api.Services;
using SportsHub.Domain.Entities;
using SportsHub.Domain.Repositories;
using SportsHub.Infrastructure.Db;

namespace SportsHub.Api.UnitTests.Services;

[TestFixture]
public class ArticlesServiceTests
{
    private IArticlesService _sut;
    private readonly List<Article> _articles = [];

    [OneTimeSetUp]
    public void SetupOnce()
    {
        var unitOfWork = MockUnitOfWork();
        var articlesRepository = MockArticlesRepository();
        var mapper = new ApplicationMapper();
        var httpContextAccessor = new HttpContextAccessor();
        _sut = new ArticlesService(unitOfWork, articlesRepository, mapper, httpContextAccessor);
    }

    [Test]
    public async Task GetArticles_NoArticles_ReturnsEmptyArray()
    {
        var response = await _sut.GetArticles();

        Assert.That(response, Is.Not.Null);
        Assert.That(response, Is.Empty);
    }

    [Test]
    public async Task GetArticles_ArticlesPresent_MapsEachArticleAndReturnsArray()
    {
        AddTestArticleToSetup("test1", "test1 short", "test1 desc");
        AddTestArticleToSetup("test2", "test2 short", "test2 desc");

        var response = await _sut.GetArticles();

        Assert.That(response, Is.Not.Null);
        Assert.That(response.Length, Is.EqualTo(2));
    }

    private static IUnitOfWork MockUnitOfWork()
    {
        var unitOfWorkMock = A.Fake<IUnitOfWork>();

        A.CallTo(() => unitOfWorkMock.ExecuteInTransactionAsync(A<Func<Task>>._))
            .Invokes((Func<Task> func) => func());

        A.CallTo(() => unitOfWorkMock.ExecuteInTransaction(A<Action>._))
            .Invokes((Action action) => action());

        return unitOfWorkMock;
    }

    private IArticlesRepository MockArticlesRepository()
    {
        var articleRepositoryMock = A.Fake<IArticlesRepository>();

        A.CallTo(() => articleRepositoryMock.GetById(A<int>._))
            .ReturnsLazily((int id) =>
            {
                return Task.FromResult(_articles.FirstOrDefault(x => x.Id == id));
            });

        A.CallTo(() => articleRepositoryMock.GetAll())
            .ReturnsLazily(() => _articles.ToList());

        return articleRepositoryMock;
    }

    private void AddTestArticleToSetup(string title, string shortDescription, string description)
    {
        var article = new Article(title, shortDescription, description);
        article.TrackCreation(Guid.NewGuid().ToString("D"));
        _articles.Add(article);
    }

    [TearDown]
    public void TearDown()
    {
        _articles.Clear();
    }
}