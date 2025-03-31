namespace SportsHub.Domain.Entities;

public class Article : AuditEntity
{
    public string Title { get; private set; }
    public string ShortDescription { get; private set; }
    public string Description { get; private set; }
    public string ImageFileName { get; private set; }
    public int ArticleLikes { get; private set; }
    public int ArticleDislikes { get; private set; }
    public List<ArticleComment> Comments { get; init; } = new();

    public Article(string title, string shortDescription, string description)
    {
        Title = title;
        ShortDescription = shortDescription;
        Description = description;
    }

    public void ApplyUpdate(string title, string shortDescription, string description)
    {
        if (!string.IsNullOrWhiteSpace(title))
        {
            Title = title;
        }

        if (!string.IsNullOrWhiteSpace(shortDescription))
        {
            ShortDescription = shortDescription;
        }

        if (!string.IsNullOrWhiteSpace(description))
        {
            Description = description;
        }
    }

    public void SetImage(string imageFileName)
    {
        ImageFileName = imageFileName;
    }

    public void UpdateReactionsData(int likes, int dislikes)
    {
        ArticleLikes = likes;
        ArticleDislikes = dislikes;
    }
}