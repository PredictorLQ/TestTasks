using Microsoft.EntityFrameworkCore;
using Umbrella.Domain.Entities;

namespace Umbrella.Infrastructure.Data;

public sealed class UmbrellaDbContext(DbContextOptions<UmbrellaDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<DataSource> DataSources { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<ScheduledReport> ScheduledReports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UmbrellaDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<Entity>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted);

        var entriesList = entries.ToList();
        for (var i = 0; i < entriesList.Count; i++)
        {
            var entry = entriesList[i];
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Id = Guid.NewGuid();
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.Version = 0;
                    break;
                case EntityState.Modified:
                    entry.Entity.ChangedAt = DateTime.UtcNow;
                    entry.Entity.Version++;
                    break;
                case EntityState.Deleted:
                    entry.Entity.Delete();
                    entry.State = EntityState.Modified;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}