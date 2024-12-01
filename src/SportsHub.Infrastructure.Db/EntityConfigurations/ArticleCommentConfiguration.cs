using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportsHub.Domain.Entities;

namespace SportsHub.Infrastructure.Db.EntityConfigurations;

public class ArticleCommentConfiguration : AuditEntityConfiguration<ArticleComment>
{
    public override void Configure(EntityTypeBuilder<ArticleComment> builder)
    {
        base.Configure(builder);

        builder.HasMany(c => c.ChildComments)
            .WithOne(c => c.ParentComment)
            .HasForeignKey(c => c.ParentCommentId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(false);
    }
}