namespace SportsHub.Domain.Entities;

public class ArticleComment : AuditEntity
{
    public Article Article { get; init; }
    public int ArticleId { get; init; }
 
    public int? ParentCommentId { get; init; }
    public ArticleComment ParentComment { get; init; }
    public List<ArticleComment> ChildComments { get; init; } = new();

    public string CommentText { get; set; }
}