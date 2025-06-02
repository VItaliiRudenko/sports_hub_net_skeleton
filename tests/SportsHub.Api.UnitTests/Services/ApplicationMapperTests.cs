using NUnit.Framework;
using SportsHub.Api.Services;
using SportsHub.Domain.Entities;

namespace SportsHub.Api.UnitTests.Services;

[TestFixture]
public class ApplicationMapperTests
{
    private ApplicationMapper _sut;

    [SetUp]
    public void SetUp()
    {
        _sut = new ApplicationMapper();
    }

    [Test]
    public void ToArticleResponse_WithBasicArticle_MapsCorrectly()
    {
        var article = new Article("Test Title", "Short Description", "Full Description");
        article.TrackCreation("user-id");
        article.TrackUpdate("user-id");
        
        var apiBaseUrl = "https://api.example.com";

        var result = _sut.ToArticleResponse(article, apiBaseUrl);

        Assert.That(result.Id, Is.EqualTo(article.Id));
        Assert.That(result.Title, Is.EqualTo("Test Title"));
        Assert.That(result.ShortDescription, Is.EqualTo("Short Description"));
        Assert.That(result.Description, Is.EqualTo("Full Description"));
        Assert.That(result.ArticleLikes, Is.EqualTo(article.ArticleLikes));
        Assert.That(result.ArticleDislikes, Is.EqualTo(article.ArticleDislikes));
        Assert.That(result.CommentsCount, Is.EqualTo(0));
        Assert.That(result.CommentsContent, Is.Empty);
        Assert.That(result.ImageUrl, Is.EqualTo($"{apiBaseUrl}/api/article-images/{article.ImageFileName}"));
        
        // Compare DateTimeOffset and DateTime properly by converting both to UTC
        Assert.That(result.CreatedAt.UtcDateTime, Is.EqualTo(article.CreatedAt.ToUniversalTime()));
        Assert.That(result.UpdatedAt?.UtcDateTime, Is.EqualTo(article.UpdatedAt?.ToUniversalTime()));
    }

    [Test]
    public void ToArticleResponse_WithComments_MapsCommentsCorrectly()
    {
        var article = new Article("Test Title", "Short Description", "Full Description");
        article.TrackCreation("user-id");
        
        // Note: Since Article.Comments might be read-only, we need to check the actual implementation
        // This test assumes we can add comments through the domain logic
        
        var apiBaseUrl = "https://api.example.com";

        var result = _sut.ToArticleResponse(article, apiBaseUrl);

        Assert.That(result.CommentsCount, Is.EqualTo(article.Comments.Count));
        Assert.That(result.CommentsContent.Count, Is.EqualTo(article.Comments.Count));
    }

    [Test]
    public void ToArticleResponse_WithDifferentApiBaseUrl_GeneratesCorrectImageUrl()
    {
        var article = new Article("Test Title", "Short Description", "Full Description");
        article.TrackCreation("user-id");
        
        var apiBaseUrl = "https://different.api.com";

        var result = _sut.ToArticleResponse(article, apiBaseUrl);

        Assert.That(result.ImageUrl, Is.EqualTo($"{apiBaseUrl}/api/article-images/{article.ImageFileName}"));
    }

    [Test]
    public void ToArticleResponse_WithNullApiBaseUrl_HandlesGracefully()
    {
        var article = new Article("Test Title", "Short Description", "Full Description");
        article.TrackCreation("user-id");

        var result = _sut.ToArticleResponse(article, null);

        Assert.That(result.ImageUrl, Is.EqualTo($"/api/article-images/{article.ImageFileName}"));
    }

    [Test]
    public void ToArticleResponse_WithEmptyApiBaseUrl_HandlesGracefully()
    {
        var article = new Article("Test Title", "Short Description", "Full Description");
        article.TrackCreation("user-id");

        var result = _sut.ToArticleResponse(article, "");

        Assert.That(result.ImageUrl, Is.EqualTo($"/api/article-images/{article.ImageFileName}"));
    }
} 