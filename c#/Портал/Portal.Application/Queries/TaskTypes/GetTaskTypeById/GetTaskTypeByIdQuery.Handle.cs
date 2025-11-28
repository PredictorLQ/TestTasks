using MediatR;
using Microsoft.Extensions.Logging;
using Portal.Application.Contracts.Services;
using Portal.Domain.Dtos;

namespace Portal.Application.Queries.TaskTypes.GetTaskTypeById;

public sealed class GetTaskTypeByIdQueryHandle(
        ITaskTypeService taskTypeService,
        ILogger<GetTaskTypeByIdQueryHandle> logger) : IRequestHandler<GetTaskTypeByIdQuery, TaskTypeDto?>
{
    public async Task<TaskTypeDto?> Handle(GetTaskTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var methodName = nameof(Handle);
        logger.LogDebug("GetTaskTypeByIdQuery.{methodName}. Получение типа задачи. TaskTypeId: {taskTypeId}",
            methodName, request.Id);

        var result = await taskTypeService.GetByIdAsync(request.Id, cancellationToken);

        if (result == null)
        {
            logger.LogWarning("GetTaskTypeByIdQuery.{methodName}. Тип задачи не найден. TaskTypeId: {taskTypeId}",
                methodName, request.Id);
        }

        return result;
    }
}