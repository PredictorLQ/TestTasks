using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using task.Entities;

namespace task.Data.Configurations;

public class OfficeConfiguration : IEntityTypeConfiguration<Office>
{
    public void Configure(EntityTypeBuilder<Office> builder)
    {
        builder.ToTable("offices");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(o => o.Code)
            .HasColumnName("code")
            .HasMaxLength(255);

        builder.Property(o => o.CityCode)
            .HasColumnName("city_code")
            .IsRequired();

        builder.Property(o => o.Uuid)
            .HasColumnName("uuid")
            .HasMaxLength(255);

        builder.Property(o => o.Type)
            .HasColumnName("type")
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(o => o.CountryCode)
            .HasColumnName("country_code")
            .HasMaxLength(10)
            .IsRequired();

        builder.OwnsOne(o => o.Coordinates, c =>
        {
            c.Property(co => co.Latitude)
                .HasColumnName("latitude")
                .IsRequired();
            c.Property(co => co.Longitude)
                .HasColumnName("longitude")
                .IsRequired();
        });

        builder.Property(o => o.AddressRegion)
            .HasColumnName("address_region")
            .HasMaxLength(255);

        builder.Property(o => o.AddressCity)
            .HasColumnName("address_city")
            .HasMaxLength(255);

        builder.Property(o => o.AddressStreet)
            .HasColumnName("address_street")
            .HasMaxLength(255);

        builder.Property(o => o.AddressHouseNumber)
            .HasColumnName("address_house_number")
            .HasMaxLength(50);

        builder.Property(o => o.AddressApartment)
            .HasColumnName("address_apartment");

        builder.Property(o => o.WorkTime)
            .HasColumnName("work_time")
            .HasMaxLength(500)
            .IsRequired();

        builder.HasMany(o => o.Phones)
            .WithOne(p => p.Office)
            .HasForeignKey(p => p.OfficeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Индексы для производительности
        builder.HasIndex(o => o.CityCode)
            .HasDatabaseName("ix_offices_city_code");

        builder.HasIndex(o => o.Code)
            .HasDatabaseName("ix_offices_code");

        builder.HasIndex(o => o.Uuid)
            .HasDatabaseName("ix_offices_uuid");
    }
}