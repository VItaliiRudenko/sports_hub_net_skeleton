namespace SportsHub.Domain.Entities;

public class Article : AuditEntity
{
    public string Title { get; init; }
    public string ShortDescription { get; init; }
    public string Description { get; init; }
    public string ImageFileName { get; init; }
    public int ArticleLikes { get; init; }
    public int ArticleDislikes { get; init; }
    public List<ArticleComment> Comments { get; init; } = new();
}