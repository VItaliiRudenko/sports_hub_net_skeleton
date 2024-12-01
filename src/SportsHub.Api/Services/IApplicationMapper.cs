using SportsHub.Api.Models.Articles;
using SportsHub.Domain.Entities;

namespace SportsHub.Api.Services;

public interface IApplicationMapper
{
    ArticleResponse ToArticleResponse(Article article);
}

internal class ApplicationMapper : IApplicationMapper
{
    public ArticleResponse ToArticleResponse(Article article)
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
            ImageUrl = "", // TODO
            CreatedAt = article.CreatedAt,
            UpdatedAt = article.UpdatedAt,
        };
    }
}