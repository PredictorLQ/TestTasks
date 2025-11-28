using MediatR;
using Microsoft.Extensions.Logging;
using Portal.Application.Contracts.Services;

namespace Portal.Application.Commands.TaskTypes.DeleteTaskType;

public sealed partial class DeleteTaskTypeCommand
{
    public sealed class DeleteTaskTypeCommandHandle(
        ITaskTypeService taskTypeService,
        ILogger<DeleteTaskTypeCommandHandle> logger) : IRequestHandler<DeleteTaskTypeCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteTaskTypeCommand request, CancellationToken cancellationToken)
        {
            var methodName = nameof(Handle);
            logger.LogInformation("DeleteTaskTypeCommand.{methodName}. Начало удаления типа задачи. TaskTypeId: {taskTypeId}",
                methodName, request.Id);

            await taskTypeService.DeleteAsync(request.Id, cancellationToken);

            logger.LogInformation("DeleteTaskTypeCommand.{methodName}. Тип задачи удален. TaskTypeId: {taskTypeId}",
                methodName, request.Id);

            return Unit.Value;
        }
    }
}