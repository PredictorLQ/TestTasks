using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Umbrella.Domain.Entities;

namespace Umbrella.Infrastructure.Data.Configurations;

public sealed class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(r => r.Description)
            .HasMaxLength(1000);

        builder.Property(r => r.Content)
            .IsRequired();

        builder.Property(r => r.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasOne(r => r.DataSource)
            .WithMany()
            .HasForeignKey(r => r.DataSourceId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}