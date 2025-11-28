using Microsoft.EntityFrameworkCore;
using Portal.Application.Contracts.Repositories;
using Portal.Infrastructure.Data;
using TaskEntity = Portal.Domain.Entities.Task;

namespace Portal.Infrastructure.Repositories;

public sealed class TaskRepository(PortalDbContext context) : ITaskRepository
{
    public Task<TaskEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.Tasks
            .Include(t => t.TaskType)
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted, cancellationToken);
    }

    public Task<List<TaskEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return context.Tasks
            .Include(t => t.TaskType)
            .Where(t => !t.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<TaskEntity> CreateAsync(TaskEntity task, CancellationToken cancellationToken = default)
    {
        await context.Tasks.AddAsync(task, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return task;
    }

    public async Task<TaskEntity> UpdateAsync(TaskEntity task, CancellationToken cancellationToken = default)
    {
        context.Tasks.Update(task);
        await context.SaveChangesAsync(cancellationToken);
        return task;
    }

    public async System.Threading.Tasks.Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var task = await GetByIdAsync(id, cancellationToken);
        if (task != null)
        {
            task.Delete();
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.Tasks
            .AnyAsync(t => t.Id == id && !t.IsDeleted, cancellationToken);
    }
}