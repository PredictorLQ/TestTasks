using MediatR;
using Microsoft.Extensions.Logging;
using Portal.Application.Contracts.Services;
using Portal.Domain.Dtos;

namespace Portal.Application.Queries.TaskTypes.GetAllTaskTypes;

public sealed class GetAllTaskTypesQueryHandle(
        ITaskTypeService taskTypeService,
        ILogger<GetAllTaskTypesQueryHandle> logger) : IRequestHandler<GetAllTaskTypesQuery, List<TaskTypeDto>>
{
    public async Task<List<TaskTypeDto>> Handle(GetAllTaskTypesQuery request, CancellationToken cancellationToken)
    {
        var methodName = nameof(Handle);
        logger.LogDebug("GetAllTaskTypesQuery.{methodName}. Получение всех типов задач", methodName);

        var result = await taskTypeService.GetAllAsync(cancellationToken);

        logger.LogDebug("GetAllTaskTypesQuery.{methodName}. Получено типов задач: {count}",
            methodName, result.Count);

        return result;
    }
}