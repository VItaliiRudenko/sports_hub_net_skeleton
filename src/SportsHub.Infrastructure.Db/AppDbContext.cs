using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SportsHub.Domain.Entities;
using SportsHub.Domain.Services;
using SportsHub.Infrastructure.Db.EntityConfigurations;

namespace SportsHub.Infrastructure.Db;

public class AppDbContext : IdentityDbContext
{
    private readonly IContextDataProvider _contextDataProvider;

    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        IContextDataProvider contextDataProvider)
        : base(options)
    {
        _contextDataProvider = contextDataProvider;
    }

    public DbSet<Article> Articles { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AuditEntityConfiguration<>).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetAuditValues();
        return await base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        SetAuditValues();
        return base.SaveChanges();
    }
    
    private void SetAuditValues()
    {
        var entries = ChangeTracker.Entries<AuditEntity>().ToList();

        if (!entries.Any(x => x.State is EntityState.Added or EntityState.Modified))
        {
            return;
        }

        var userId = _contextDataProvider.GetCurrentUserId();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.CreatedByUserId = userId;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedByUserId = userId;
                    break;
            }
        }
    }
}