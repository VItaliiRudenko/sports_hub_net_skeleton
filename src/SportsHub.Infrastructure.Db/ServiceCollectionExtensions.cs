using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SportsHub.Domain.Repositories;
using SportsHub.Domain.Services;
using SportsHub.Infrastructure.Db.Repositories;
using SportsHub.Infrastructure.Db.Services;

namespace SportsHub.Infrastructure.Db;

public static class ServiceCollectionExtensions
{
    public static void AddDbServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(o =>
            o.UseNpgsql(
                    configuration.GetConnectionString("SportsHubDb"),
                    m => m.MigrationsAssembly("SportsHub.Infrastructure.Db.Migrations"))
                .UseSnakeCaseNamingConvention());

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IArticlesRepository, ArticlesRepository>();
        services.AddScoped<IJwtDenyListRepository, JwtDenyListRepository>();

        services.AddScoped<IFileStorage, DbFileStorage>();
    }
}