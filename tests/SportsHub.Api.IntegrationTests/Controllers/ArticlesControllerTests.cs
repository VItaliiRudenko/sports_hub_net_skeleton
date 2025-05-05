using System.Net;
using System.Net.Http.Json;
using NUnit.Framework;
using SportsHub.Api.Models.Articles;

namespace SportsHub.Api.IntegrationTests.Controllers;

[TestFixture]
public class ArticlesControllerTests
{
    [Test]
    public async Task GetList_AnonymousUser_ReturnsArticlesList()
    {
        var client = SystemUnderTest.GetAnonymousHttpClient();
        var response = await client.GetAsync("/api/articles");

        var articles = await response.Content.ReadFromJsonAsync<ArticleResponse[]>(SystemUnderTest.DefaultJsonOptions);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(articles, Is.Not.Null);
        Assert.That(articles.Length, Is.Not.Zero);
    }

    [Test]
    public async Task GetById_AnonymousUser_ReturnsArticlesItem()
    {
        var client = SystemUnderTest.GetAnonymousHttpClient();
        var response = await client.GetAsync("/api/articles/1");

        var article = await response.Content.ReadFromJsonAsync<ArticleResponse>(SystemUnderTest.DefaultJsonOptions);

        using var _ = Assert.EnterMultipleScope();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(article, Is.Not.Null);
        Assert.That(article.Id, Is.EqualTo(1));
    }

    [Test]
    public async Task CreateArticle_AnonymousUser_ReturnsUnauthorized()
    {
        var client = SystemUnderTest.GetAnonymousHttpClient();
        var response = await client.PostAsJsonAsync("/api/articles", new CreateArticleRequest());

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task CreateArticle_AuthorizedUserButInvalidModel_ReturnsBadRequest()
    {
        var client = await SystemUnderTest.GetAuthorizedHttpClient();
        var response = await client.PostAsJsonAsync("/api/articles", new CreateArticleRequest());

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task CreateArticle_AuthorizedUserAndValidModel_CreatesArticle()
    {
        var client = await SystemUnderTest.GetAuthorizedHttpClient();
        const string newTitle = "New Title";
        const string newDescription = "New Description";


        var response = await client.PostAsJsonAsync("/api/articles", new CreateArticleRequest
        {
            Title = newTitle,
            Description = newDescription,
            ShortDescription = newDescription,
        }, SystemUnderTest.DefaultJsonOptions);

        var article = await response.Content.ReadFromJsonAsync<ArticleResponse>(SystemUnderTest.DefaultJsonOptions);

        using var _ = Assert.EnterMultipleScope();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(article, Is.Not.Null);
        Assert.That(article.Id, Is.Not.Zero);
        Assert.That(article.Title, Is.EqualTo(newTitle));
        Assert.That(article.Description, Is.EqualTo(newDescription));
        Assert.That(article.ShortDescription, Is.EqualTo(newDescription));
    }
}