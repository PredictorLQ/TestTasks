using Microsoft.EntityFrameworkCore;
using Portal.Application.Contracts.Repositories;
using Portal.Domain.Entities;
using Portal.Infrastructure.Data;

namespace Portal.Infrastructure.Repositories;

public sealed class TaskTypeRepository(PortalDbContext context) : ITaskTypeRepository
{
    public Task<TaskType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.TaskTypes
            .FirstOrDefaultAsync(tt => tt.Id == id && !tt.IsDeleted, cancellationToken);
    }

    public Task<List<TaskType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return context.TaskTypes
            .Where(tt => !tt.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.TaskTypes
            .AnyAsync(tt => tt.Id == id && !tt.IsDeleted, cancellationToken);
    }

    public async Task<TaskType> CreateAsync(TaskType taskType, CancellationToken cancellationToken = default)
    {
        await context.TaskTypes.AddAsync(taskType, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return taskType;
    }

    public async Task<TaskType> UpdateAsync(TaskType taskType, CancellationToken cancellationToken = default)
    {
        context.TaskTypes.Update(taskType);
        await context.SaveChangesAsync(cancellationToken);
        return taskType;
    }

    public async System.Threading.Tasks.Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var taskType = await GetByIdAsync(id, cancellationToken);
        if (taskType != null)
        {
            taskType.Delete();
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}