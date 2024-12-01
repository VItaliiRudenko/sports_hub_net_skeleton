using Microsoft.AspNetCore.Identity;

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
        var usersCount = _appDbContext.Users.Count();

        if (usersCount > 0)
        {
            return;
        }

        var email = "test@gmail.com";
        var password = "password1";

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

        _appDbContext.SaveChanges();
    }
}