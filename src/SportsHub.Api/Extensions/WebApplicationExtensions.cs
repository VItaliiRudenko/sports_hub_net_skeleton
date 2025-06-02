using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SportsHub.Infrastructure.Db;
using SportsHub.Infrastructure.Db.DataSeed;

namespace SportsHub.Api.Extensions;

public static class WebApplicationExtensions
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
    }

    public static async Task SeedInitialData(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        var seeder = new InitialDataSeeder(dbContext, roleManager, userManager);
        await seeder.SeedData();
    }
}