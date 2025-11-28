using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.Domain.Entities;

namespace Portal.Infrastructure.Data.Configurations;

public sealed class TaskTypeConfiguration : IEntityTypeConfiguration<TaskType>
{
    public void Configure(EntityTypeBuilder<TaskType> builder)
    {
        builder.ToTable("TaskTypes");

        builder.HasKey(tt => tt.Id);

        builder.Property(tt => tt.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(tt => tt.Description)
            .HasMaxLength(500);

        builder.Property(tt => tt.Id)
            .IsRequired();

        builder.Property(tt => tt.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(tt => tt.CreatedAt)
            .IsRequired();

        builder.Property(tt => tt.Version)
            .IsRequired()
            .HasDefaultValue(0L);

        builder.HasQueryFilter(tt => !tt.IsDeleted);
    }
}