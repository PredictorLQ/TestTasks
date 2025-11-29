using Microsoft.EntityFrameworkCore;
using Umbrella.Application.Contracts.Repositories;
using Umbrella.Domain.Entities;
using Umbrella.Infrastructure.Data;

namespace Umbrella.Infrastructure.Repositories;

public sealed class ReportRepository(UmbrellaDbContext context) : IReportRepository
{
    public Task<Report?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.Reports
            .Include(r => r.DataSource)
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted, cancellationToken);
    }

    public Task<List<Report>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return context.Reports
            .Include(r => r.DataSource)
            .Where(r => !r.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<Report> CreateAsync(Report report, CancellationToken cancellationToken = default)
    {
        await context.Reports.AddAsync(report, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return report;
    }

    public async Task<Report> UpdateAsync(Report report, CancellationToken cancellationToken = default)
    {
        context.Reports.Update(report);
        await context.SaveChangesAsync(cancellationToken);
        return report;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var report = await GetByIdAsync(id, cancellationToken);
        if (report != null)
        {
            report.Delete();
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.Reports
            .AnyAsync(r => r.Id == id && !r.IsDeleted, cancellationToken);
    }
}