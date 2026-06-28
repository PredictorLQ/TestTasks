using Microsoft.EntityFrameworkCore;
using OrderTrackingApplication.Application.Contracts.Repositories;
using OrderTrackingApplication.Domain.Entities;
using OrderTrackingApplication.Infrastructure.Data;

namespace OrderTrackingApplication.Infrastructure.Repositories;

public sealed class OutboxRepository(OrderDbContext dbContext) : IOutboxRepository
{
    public async Task AddAsync(OutboxMessage message, CancellationToken cancellationToken = default)
    {
        await dbContext.OutboxMessages.AddAsync(message, cancellationToken);
    }

    public async Task<IReadOnlyList<OutboxMessage>> GetPendingAsync(int batchSize, CancellationToken cancellationToken = default)
    {
        return await dbContext.OutboxMessages
            .Where(x => x.ProcessedAt == null && x.RetryCount < 5)
            .OrderBy(x => x.CreatedAt)
            .Take(batchSize)
            .ToListAsync(cancellationToken);
    }

    public async Task MarkAsProcessedAsync(Guid messageId, CancellationToken cancellationToken = default)
    {
        var message = await dbContext.OutboxMessages
            .FirstOrDefaultAsync(x => x.Id == messageId, cancellationToken);

        if (message is null)
        {
            return;
        }

        message.ProcessedAt = DateTime.UtcNow;
        message.LastError = null;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task MarkAsFailedAsync(Guid messageId, string error, CancellationToken cancellationToken = default)
    {
        var message = await dbContext.OutboxMessages
            .FirstOrDefaultAsync(x => x.Id == messageId, cancellationToken);

        if (message is null)
        {
            return;
        }

        message.RetryCount++;
        message.LastError = error;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
