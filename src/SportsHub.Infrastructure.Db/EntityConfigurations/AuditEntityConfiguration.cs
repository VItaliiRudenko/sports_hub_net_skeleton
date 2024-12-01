using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportsHub.Domain.Entities;

namespace SportsHub.Infrastructure.Db.EntityConfigurations;

public class AuditEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : AuditEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);
    }
}