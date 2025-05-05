using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SportsHub.Infrastructure.Db;

namespace SportsHub.Api.IntegrationTests;

internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureTestServices(services =>
        {
            RemoveImplIfExists<DbContextOptions<AppDbContext>>(services);
            RemoveImplIfExists<DbConnection>(services);

            services.AddSingleton<DbConnection>(container =>
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                return connection;
            });

            services.AddDbContext<AppDbContext>((provider, options) =>
            {
                var connection = provider.GetRequiredService<DbConnection>();
                options.UseSqlite(
                    connection,
                    m => m.MigrationsAssembly("SportsHub.Infrastructure.Db.Migrations"))
                    .UseSnakeCaseNamingConvention();
            });
        });
    }

    private static void RemoveImplIfExists<T>(IServiceCollection services)
    {
        var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(T));
        if (descriptor is not null)
        {
            services.Remove(descriptor);
        }
    }
}