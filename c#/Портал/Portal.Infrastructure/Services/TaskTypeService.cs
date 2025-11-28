using Mapster;
using Microsoft.Extensions.Logging;
using Portal.Application.Contracts.Repositories;
using Portal.Application.Contracts.Services;
using Portal.Domain.Dtos;
using Portal.Domain.Entities;
using Portal.Domain.Models;

namespace Portal.Infrastructure.Services;

public sealed class TaskTypeService(
    ITaskTypeRepository taskTypeRepository,
    ILogger<TaskTypeService> logger) : ITaskTypeService
{
    private readonly string _serviceName = nameof(TaskTypeService);

    public async Task<TaskTypeDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var methodName = nameof(GetByIdAsync);
        logger.LogDebug("{serviceName}.{methodName}. Получение типа задачи. TaskTypeId: {taskTypeId}",
            _serviceName, methodName, id);

        var taskType = await taskTypeRepository.GetByIdAsync(id, cancellationToken);
        if (taskType == null)
        {
            logger.LogWarning("{serviceName}.{methodName}. Тип задачи не найден. TaskTypeId: {taskTypeId}",
                _serviceName, methodName, id);
            return null;
        }

        return taskType.Adapt<TaskTypeDto>();
    }

    public async Task<List<TaskTypeDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var methodName = nameof(GetAllAsync);
        logger.LogDebug("{serviceName}.{methodName}. Получение всех типов задач", _serviceName, methodName);

        var taskTypes = await taskTypeRepository.GetAllAsync(cancellationToken);
        return taskTypes.Adapt<List<TaskTypeDto>>();
    }

    public async Task<TaskTypeDto> CreateAsync(TaskTypeModel model, CancellationToken cancellationToken = default)
    {
        var methodName = nameof(CreateAsync);
        logger.LogInformation("{serviceName}.{methodName}. Создание типа задачи. Name: {name}",
            _serviceName, methodName, model.Name);

        var taskType = model.Adapt<TaskType>();

        var createdTaskType = await taskTypeRepository.CreateAsync(taskType, cancellationToken);

        logger.LogInformation("{serviceName}.{methodName}. Тип задачи создан. TaskTypeId: {taskTypeId}",
            _serviceName, methodName, createdTaskType.Id);

        return createdTaskType.Adapt<TaskTypeDto>();
    }

    public async Task<TaskTypeDto> UpdateAsync(Guid id, TaskTypeModel model, CancellationToken cancellationToken = default)
    {
        var methodName = nameof(UpdateAsync);
        logger.LogInformation("{serviceName}.{methodName}. Обновление типа задачи. TaskTypeId: {taskTypeId}",
            _serviceName, methodName, id);

        var taskType = await taskTypeRepository.GetByIdAsync(id, cancellationToken);
        if (taskType == null)
        {
            logger.LogError("{serviceName}.{methodName}. Тип задачи не найден. TaskTypeId: {taskTypeId}",
                _serviceName, methodName, id);
            throw new InvalidOperationException($"Тип задачи с Id {id} не найден");
        }

        model.Adapt(taskType);

        var updatedTaskType = await taskTypeRepository.UpdateAsync(taskType, cancellationToken);

        logger.LogInformation("{serviceName}.{methodName}. Тип задачи обновлен. TaskTypeId: {taskTypeId}",
            _serviceName, methodName, updatedTaskType.Id);

        return updatedTaskType.Adapt<TaskTypeDto>();
    }

    public async System.Threading.Tasks.Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var methodName = nameof(DeleteAsync);
        logger.LogInformation("{serviceName}.{methodName}. Удаление типа задачи. TaskTypeId: {taskTypeId}",
            _serviceName, methodName, id);

        await taskTypeRepository.DeleteAsync(id, cancellationToken);

        logger.LogInformation("{serviceName}.{methodName}. Тип задачи удален. TaskTypeId: {taskTypeId}",
            _serviceName, methodName, id);
    }
}