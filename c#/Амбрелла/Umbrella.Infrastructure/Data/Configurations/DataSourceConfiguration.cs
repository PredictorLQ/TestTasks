using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Umbrella.Domain.Entities;
using Umbrella.Domain.Enums;
using Umbrella.Infrastructure.Extensions;

namespace Umbrella.Infrastructure.Data.Configurations;

public sealed class DataSourceConfiguration : IEntityTypeConfiguration<DataSource>
{
    public void Configure(EntityTypeBuilder<DataSource> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.Type)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => v.ParseEnum<DataSourceType>());

        builder.Property(d => d.ConnectionString)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(d => d.Description)
            .HasMaxLength(1000);

        builder.Property(d => d.IsActive)
            .IsRequired()
            .HasDefaultValue(true);
    }
}