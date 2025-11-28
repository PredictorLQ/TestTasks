using Microsoft.EntityFrameworkCore;
using Portal.Domain.Entities;
using TaskEntity = Portal.Domain.Entities.Task;

namespace Portal.Infrastructure.Data;

public sealed class PortalDbContext(DbContextOptions<PortalDbContext> options) : DbContext(options)
{
    public DbSet<TaskEntity> Tasks { get; set; }
    public DbSet<TaskType> TaskTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PortalDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<Entity>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted);

        foreach (var entry in entries)
        {
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