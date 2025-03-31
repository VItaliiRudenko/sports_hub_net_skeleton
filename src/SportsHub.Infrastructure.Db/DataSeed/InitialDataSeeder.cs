using Microsoft.AspNetCore.Identity;
using SportsHub.Domain.Entities;

namespace SportsHub.Infrastructure.Db.DataSeed;

public class InitialDataSeeder
{
    private readonly AppDbContext _appDbContext;

    public InitialDataSeeder(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public void SeedData()
    {
        SeedUsers();
        SeedArticles();
        SeedImages();
    }

    private void SeedUsers()
    {
        var usersCount = _appDbContext.Users.Count();

        if (usersCount > 0)
        {
            return;
        }

        SeedUser("test1@gmail.com", "password1");
        SeedUser("test2@gmail.com", "password2");

        _appDbContext.SaveChanges();
    }

    private void SeedUser(string email, string password)
    {
        var user = new IdentityUser
        {
            Id = Guid.NewGuid().ToString(),
            Email = email,
            NormalizedEmail = email.ToUpperInvariant(),
            UserName = email,
            NormalizedUserName = email.ToUpperInvariant(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("N").ToUpper(),
        };

        var passwordHasher = new PasswordHasher<IdentityUser>();
        var passwordHash = passwordHasher.HashPassword(user, password);

        user.PasswordHash = passwordHash;

        _appDbContext.Users.Add(user);
    }

    private void SeedArticles()
    {
        var articlesCount = _appDbContext.Articles.Count();
        if (articlesCount > 0)
        {
            return;
        }

        var userId = _appDbContext.Users.Select(u => u.Id).FirstOrDefault();

        SeedArticle("The Art of Scoring Goals", "Learn how to score goals like a pro with this comprehensive guide to the art of goal-scoring.", "1.jpg", userId);
        SeedArticle("Mastering the Perfect Serve", "Master the perfect serve with this step-by-step guide to serving in tennis.", "2.jpg", userId);
        SeedArticle("Unleashing Your Inner Athlete", "Unleash your inner athlete with this guide to becoming the best athlete you can be.", "3.jpg", userId);
        SeedArticle("The Science of Sports Performance", "Discover the science behind sports performance and how to improve your game.", "4.jpg", userId);
        SeedArticle("Achieving Victory Through Teamwork", "Achieve victory through teamwork with this guide to working together as a team.", "5.jpg", userId);
        SeedArticle("Exploring the World of Extreme Sports", "Explore the world of extreme sports and learn how to get started with this guide.", "6.jpg", userId);
    }

    private static readonly string[] ArticleComments = {
        "This article was very informative and helpful.",
        "I learned a lot from this article and will definitely be using the tips.",
        "Great article! I will be sharing this with my friends.",
        "I loved this article and will be reading more from this author.",
        "This article was very well-written and easy to understand.",
        "I will be recommending this article to everyone I know.",
        "I learned so much from this article and will be using the tips in my own life.",
        "This article was very helpful and informative.",
        "I loved this article and will be reading more from this author.",
        "I will be recommending this article to everyone I know.",
    };

    private void SeedArticle(string title, string shortDescription, string imageFileName, string userId)
    {
        var article = new Article(title, shortDescription,
            $"This is the description of the article with title: {title}");

        article.UpdateReactionsData(Random.Shared.Next(44, 66), Random.Shared.Next(11, 20));

        var commentsCount = Random.Shared.Next(2, 6);

        for (var i = 0; i < commentsCount; ++i)
        {
            article.Comments.Add(new ArticleComment
            {
                CommentText = ArticleComments[Random.Shared.Next(0, ArticleComments.Length)],
            });
        }

        article.TrackCreation(userId);
        article.SetImage(imageFileName);

        _appDbContext.Articles.Add(article);

        _appDbContext.SaveChanges();
    }

    private void SeedImages()
    {
        var filesCount = _appDbContext.FileItems.Count();
        if (filesCount > 0)
        {
            return;
        }

        _appDbContext.FileItems.Add(new FileItem("1.jpg", "image/jpg", ImageFilesToSeed.File1));
        _appDbContext.FileItems.Add(new FileItem("2.jpg", "image/jpg", ImageFilesToSeed.File2));
        _appDbContext.FileItems.Add(new FileItem("3.jpg", "image/jpg", ImageFilesToSeed.File3));
        _appDbContext.FileItems.Add(new FileItem("4.jpg", "image/jpg", ImageFilesToSeed.File4));
        _appDbContext.FileItems.Add(new FileItem("5.jpg", "image/jpg", ImageFilesToSeed.File5));
        _appDbContext.FileItems.Add(new FileItem("6.jpg", "image/jpg", ImageFilesToSeed.File6));

        _appDbContext.SaveChanges();
    }
}