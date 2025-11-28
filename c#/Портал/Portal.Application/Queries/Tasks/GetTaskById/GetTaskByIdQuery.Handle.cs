using MediatR;
using Microsoft.Extensions.Logging;
using Portal.Application.Contracts.Services;
using Portal.Domain.Dtos;

namespace Portal.Application.Queries.Tasks.GetTaskById;

public sealed partial class GetTaskByIdQuery
{
    public sealed class GetTaskByIdQueryHandle(
        ITaskService taskService,
        ILogger<GetTaskByIdQueryHandle> logger) : IRequestHandler<GetTaskByIdQuery, TaskDto?>
    {
        public async Task<TaskDto?> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var methodName = nameof(Handle);
            logger.LogDebug("GetTaskByIdQuery.{methodName}. Получение задачи. TaskId: {taskId}",
                methodName, request.Id);

            var result = await taskService.GetByIdAsync(request.Id, cancellationToken);

            if (result == null)
            {
                logger.LogWarning("GetTaskByIdQuery.{methodName}. Задача не найдена. TaskId: {taskId}",
                    methodName, request.Id);
            }

            return result;
        }
    }
}