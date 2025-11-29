using Microsoft.EntityFrameworkCore;
using Umbrella.Application.Contracts.Repositories;
using Umbrella.Domain.Entities;
using Umbrella.Infrastructure.Data;

namespace Umbrella.Infrastructure.Repositories;

public sealed class ScheduledReportRepository(UmbrellaDbContext context) : IScheduledReportRepository
{
    public Task<ScheduledReport?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.ScheduledReports
            .Include(s => s.Report)
            .Include(s => s.DataSource)
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted, cancellationToken);
    }

    public Task<List<ScheduledReport>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return context.ScheduledReports
            .Include(s => s.Report)
            .Include(s => s.DataSource)
            .Where(s => !s.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<ScheduledReport> CreateAsync(ScheduledReport scheduledReport, CancellationToken cancellationToken = default)
    {
        await context.ScheduledReports.AddAsync(scheduledReport, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return scheduledReport;
    }

    public async Task<ScheduledReport> UpdateAsync(ScheduledReport scheduledReport, CancellationToken cancellationToken = default)
    {
        context.ScheduledReports.Update(scheduledReport);
        await context.SaveChangesAsync(cancellationToken);
        return scheduledReport;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var scheduledReport = await GetByIdAsync(id, cancellationToken);
        if (scheduledReport != null)
        {
            scheduledReport.Delete();
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.ScheduledReports
            .AnyAsync(s => s.Id == id && !s.IsDeleted, cancellationToken);
    }
}