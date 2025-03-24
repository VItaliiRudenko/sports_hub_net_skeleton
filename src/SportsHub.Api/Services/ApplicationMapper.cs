using SportsHub.Api.Models.Articles;
using SportsHub.Domain.Entities;

namespace SportsHub.Api.Services;

internal class ApplicationMapper : IApplicationMapper
{
    public ArticleResponse ToArticleResponse(Article article, string apiBaseUrl)
    {
        return new ArticleResponse
        {
            Id = article.Id,
            Title = article.Title,
            ShortDescription = article.ShortDescription,
            Description = article.Description,
            ArticleLikes = article.ArticleLikes,
            ArticleDislikes = article.ArticleDislikes,
            CommentsCount = article.Comments.Count,
            CommentsContent = article.Comments.Select(c => c.CommentText).ToList(),
            ImageUrl = $"{apiBaseUrl}/api/article-images/{article.ImageFileName}",
            CreatedAt = article.CreatedAt,
            UpdatedAt = article.UpdatedAt,
        };
    }
}