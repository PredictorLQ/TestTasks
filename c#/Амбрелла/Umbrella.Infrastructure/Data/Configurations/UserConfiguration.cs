using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Umbrella.Domain.Entities;

namespace Umbrella.Infrastructure.Data.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.KeycloakId)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(u => u.KeycloakId)
            .IsUnique();

        builder.HasIndex(u => u.Email)
            .IsUnique();
    }
}