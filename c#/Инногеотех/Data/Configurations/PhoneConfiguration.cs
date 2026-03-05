using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using task.Entities;

namespace task.Data.Configurations;

public class PhoneConfiguration : IEntityTypeConfiguration<Phone>
{
    public void Configure(EntityTypeBuilder<Phone> builder)
    {
        builder.ToTable("phones");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.OfficeId)
            .HasColumnName("office_id")
            .IsRequired();

        builder.Property(p => p.PhoneNumber)
            .HasColumnName("phone_number")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.Additional)
            .HasColumnName("additional")
            .HasMaxLength(255);

        builder.HasIndex(p => p.OfficeId)
            .HasDatabaseName("ix_phones_office_id");
    }
}