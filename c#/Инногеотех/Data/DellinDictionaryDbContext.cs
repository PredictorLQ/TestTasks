using Microsoft.EntityFrameworkCore;
using System.Reflection;
using task.Entities;

namespace task.Data;

public class DellinDictionaryDbContext : DbContext
{
    public DbSet<Office> Offices { get; set; }

    public DellinDictionaryDbContext(DbContextOptions<DellinDictionaryDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}