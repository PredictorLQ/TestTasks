using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Portal.Application.Contracts.Services;
using Portal.Domain.Dtos;
using Portal.Domain.Models;

namespace Portal.Application.Commands.TaskTypes.UpdateTaskType;

public sealed partial class UpdateTaskTypeCommand
{
    public sealed class UpdateTaskTypeCommandHandle(
        ITaskTypeService taskTypeService,
        ILogger<UpdateTaskTypeCommandHandle> logger) : IRequestHandler<UpdateTaskTypeCommand, TaskTypeDto>
    {
        public async Task<TaskTypeDto> Handle(UpdateTaskTypeCommand request, CancellationToken cancellationToken)
        {
            var methodName = nameof(Handle);
            logger.LogInformation("UpdateTaskTypeCommand.{methodName}. Начало обновления типа задачи. TaskTypeId: {taskTypeId}",
                methodName, request.Id);

            var model = request.Adapt<TaskTypeModel>();

            var result = await taskTypeService.UpdateAsync(request.Id, model, cancellationToken);

            logger.LogInformation("UpdateTaskTypeCommand.{methodName}. Тип задачи обновлен. TaskTypeId: {taskTypeId}",
                methodName, result.Id);

            return result;
        }
    }
}