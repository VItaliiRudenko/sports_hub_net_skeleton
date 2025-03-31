using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportsHub.Domain.Entities;

namespace SportsHub.Infrastructure.Db.EntityConfigurations;

public class JwtDenyRecordConfiguration : IEntityTypeConfiguration<JwtDenyRecord>
{
    public void Configure(EntityTypeBuilder<JwtDenyRecord> builder)
    {
        builder.HasKey(x => x.Jti);
    }
}