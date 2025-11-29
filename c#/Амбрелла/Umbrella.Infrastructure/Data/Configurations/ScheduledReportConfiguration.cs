using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Umbrella.Domain.Entities;

namespace Umbrella.Infrastructure.Data.Configurations;

public sealed class ScheduledReportConfiguration : IEntityTypeConfiguration<ScheduledReport>
{
    public void Configure(EntityTypeBuilder<ScheduledReport> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.Description)
            .HasMaxLength(1000);

        builder.Property(s => s.Schedule)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Parameters)
            .HasMaxLength(5000);

        builder.Property(s => s.LastResult)
            .HasMaxLength(10000);

        builder.Property(s => s.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasOne(s => s.Report)
            .WithMany()
            .HasForeignKey(s => s.ReportId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.DataSource)
            .WithMany()
            .HasForeignKey(s => s.DataSourceId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}