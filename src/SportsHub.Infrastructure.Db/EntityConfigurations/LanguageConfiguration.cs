using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportsHub.Domain.Entities;

namespace SportsHub.Infrastructure.Db.EntityConfigurations;

public class LanguageConfiguration : AuditEntityConfiguration<Language>
{
    public override void Configure(EntityTypeBuilder<Language> builder)
    {
        base.Configure(builder);

        builder.Property(l => l.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(l => l.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder.HasIndex(l => l.Code)
            .IsUnique();
    }
} 