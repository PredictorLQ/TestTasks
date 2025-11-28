using MediatR;
using Microsoft.Extensions.Logging;
using Portal.Application.Contracts.Services;
using Portal.Domain.Dtos;

namespace Portal.Application.Queries.Tasks.GetAllTasks;

public sealed partial class GetAllTasksQuery
{
    public sealed class GetAllTasksQueryHandle(
        ITaskService taskService,
        ILogger<GetAllTasksQueryHandle> logger) : IRequestHandler<GetAllTasksQuery, List<TaskDto>>
    {
        public async Task<List<TaskDto>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            var methodName = nameof(Handle);
            logger.LogDebug("GetAllTasksQuery.{methodName}. Получение всех задач", methodName);

            var result = await taskService.GetAllAsync(cancellationToken);

            logger.LogDebug("GetAllTasksQuery.{methodName}. Получено задач: {count}",
                methodName, result.Count);

            return result;
        }
    }
}