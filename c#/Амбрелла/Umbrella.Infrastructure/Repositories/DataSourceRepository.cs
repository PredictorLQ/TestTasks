using Microsoft.EntityFrameworkCore;
using Umbrella.Application.Contracts.Repositories;
using Umbrella.Domain.Entities;
using Umbrella.Infrastructure.Data;

namespace Umbrella.Infrastructure.Repositories;

public sealed class DataSourceRepository(UmbrellaDbContext context) : IDataSourceRepository
{
    public Task<DataSource?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.DataSources
            .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted, cancellationToken);
    }

    public Task<List<DataSource>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return context.DataSources
            .Where(d => !d.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<DataSource> CreateAsync(DataSource dataSource, CancellationToken cancellationToken = default)
    {
        await context.DataSources.AddAsync(dataSource, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return dataSource;
    }

    public async Task<DataSource> UpdateAsync(DataSource dataSource, CancellationToken cancellationToken = default)
    {
        context.DataSources.Update(dataSource);
        await context.SaveChangesAsync(cancellationToken);
        return dataSource;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dataSource = await GetByIdAsync(id, cancellationToken);
        if (dataSource != null)
        {
            dataSource.Delete();
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.DataSources
            .AnyAsync(d => d.Id == id && !d.IsDeleted, cancellationToken);
    }
}