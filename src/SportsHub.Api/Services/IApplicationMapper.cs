using SportsHub.Api.Models.Articles;
using SportsHub.Domain.Entities;

namespace SportsHub.Api.Services;

public interface IApplicationMapper
{
    ArticleResponse ToArticleResponse(Article article, string apiBaseUrl);
}