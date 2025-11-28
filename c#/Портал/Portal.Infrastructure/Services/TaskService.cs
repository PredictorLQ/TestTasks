using Mapster;
using Microsoft.Extensions.Logging;
using Portal.Application.Contracts.Repositories;
using Portal.Application.Contracts.Services;
using Portal.Domain.Dtos;
using Portal.Domain.Models;
using TaskEntity = Portal.Domain.Entities.Task;

namespace Portal.Infrastructure.Services;

public sealed class TaskService(
    ITaskRepository taskRepository,
    ITaskTypeRepository taskTypeRepository,
    ILogger<TaskService> logger) : ITaskService
{
    private readonly string _serviceName = nameof(TaskService);

    public async Task<TaskDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var methodName = nameof(GetByIdAsync);
        logger.LogDebug("{serviceName}.{methodName}. Получение задачи. TaskId: {taskId}",
            _serviceName, methodName, id);

        var task = await taskRepository.GetByIdAsync(id, cancellationToken);
        if (task == null)
        {
            logger.LogWarning("{serviceName}.{methodName}. Задача не найдена. TaskId: {taskId}",
                _serviceName, methodName, id);
            return null;
        }

        return task.Adapt<TaskDto>();
    }

    public async Task<List<TaskDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var methodName = nameof(GetAllAsync);
        logger.LogDebug("{serviceName}.{methodName}. Получение всех задач", _serviceName, methodName);

        var tasks = await taskRepository.GetAllAsync(cancellationToken);
        return tasks.Adapt<List<TaskDto>>();
    }

    public async Task<TaskDto> CreateAsync(TaskModel model, CancellationToken cancellationToken = default)
    {
        var methodName = nameof(CreateAsync);
        logger.LogInformation("{serviceName}.{methodName}. Создание задачи. TaskTypeId: {taskTypeId}",
            _serviceName, methodName, model.TaskTypeId);

        if (!await taskTypeRepository.ExistsAsync(model.TaskTypeId, cancellationToken))
        {
            logger.LogError("{serviceName}.{methodName}. Тип задачи не найден. TaskTypeId: {taskTypeId}",
                _serviceName, methodName, model.TaskTypeId);
            throw new InvalidOperationException($"Тип задачи с Id {model.TaskTypeId} не найден");
        }

        var task = model.Adapt<TaskEntity>();

        var createdTask = await taskRepository.CreateAsync(task, cancellationToken);

        logger.LogInformation("{serviceName}.{methodName}. Задача создана. TaskId: {taskId}",
            _serviceName, methodName, createdTask.Id);

        return createdTask.Adapt<TaskDto>();
    }

    public async Task<TaskDto> UpdateAsync(Guid id, TaskModel model, CancellationToken cancellationToken = default)
    {
        var methodName = nameof(UpdateAsync);
        logger.LogInformation("{serviceName}.{methodName}. Обновление задачи. TaskId: {taskId}",
            _serviceName, methodName, id);

        var task = await taskRepository.GetByIdAsync(id, cancellationToken);
        if (task == null)
        {
            logger.LogError("{serviceName}.{methodName}. Задача не найдена. TaskId: {taskId}",
                _serviceName, methodName, id);
            throw new InvalidOperationException($"Задача с Id {id} не найдена");
        }

        if (!await taskTypeRepository.ExistsAsync(model.TaskTypeId, cancellationToken))
        {
            logger.LogError("{serviceName}.{methodName}. Тип задачи не найден. TaskTypeId: {taskTypeId}",
                _serviceName, methodName, model.TaskTypeId);
            throw new InvalidOperationException($"Тип задачи с Id {model.TaskTypeId} не найден");
        }

        model.Adapt(task);

        var updatedTask = await taskRepository.UpdateAsync(task, cancellationToken);

        logger.LogInformation("{serviceName}.{methodName}. Задача обновлена. TaskId: {taskId}",
            _serviceName, methodName, updatedTask.Id);

        return updatedTask.Adapt<TaskDto>();
    }

    public async System.Threading.Tasks.Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var methodName = nameof(DeleteAsync);
        logger.LogInformation("{serviceName}.{methodName}. Удаление задачи. TaskId: {taskId}",
            _serviceName, methodName, id);

        await taskRepository.DeleteAsync(id, cancellationToken);

        logger.LogInformation("{serviceName}.{methodName}. Задача удалена. TaskId: {taskId}",
            _serviceName, methodName, id);
    }

}