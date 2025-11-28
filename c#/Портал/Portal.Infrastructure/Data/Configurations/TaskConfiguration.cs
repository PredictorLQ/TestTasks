using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.Domain.Enums;
using TaskEntity = Portal.Domain.Entities.Task;

namespace Portal.Infrastructure.Data.Configurations;

public sealed class TaskConfiguration : IEntityTypeConfiguration<TaskEntity>
{
    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        builder.ToTable("Tasks");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Description)
            .HasMaxLength(1000);

        builder.Property(t => t.Status)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<TaskStatusEnum>(v));

        builder.Property(t => t.Priority)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<TaskPriorityEnum>(v));

        builder.Property(t => t.DueDate);

        builder.HasOne(t => t.TaskType)
            .WithMany(tt => tt.Tasks)
            .HasForeignKey(t => t.TaskTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(t => t.Id)
            .IsRequired();

        builder.Property(t => t.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(t => t.CreatedAt)
            .IsRequired();

        builder.Property(t => t.Version)
            .IsRequired()
            .HasDefaultValue(0L);

        builder.HasQueryFilter(t => !t.IsDeleted);
    }
}