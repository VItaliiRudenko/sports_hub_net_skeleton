namespace SportsHub.Api.Models.Articles;

public class ArticleResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public int ArticleLikes { get; set; }
    public int ArticleDislikes { get; set; }
    public List<string> CommentsContent { get; set; } = new();
    public int CommentsCount { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}