using Microsoft.EntityFrameworkCore;
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

    public static void SeedInitialData(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var seeder = new InitialDataSeeder(dbContext);
        seeder.SeedData();
    }
}