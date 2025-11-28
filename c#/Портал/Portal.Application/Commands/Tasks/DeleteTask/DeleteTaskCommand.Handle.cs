using MediatR;
using Microsoft.Extensions.Logging;
using Portal.Application.Contracts.Services;

namespace Portal.Application.Commands.Tasks.DeleteTask;

public sealed partial class DeleteTaskCommand
{
    public sealed class DeleteTaskCommandHandle(
        ITaskService taskService,
        ILogger<DeleteTaskCommandHandle> logger) : IRequestHandler<DeleteTaskCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var methodName = nameof(Handle);
            logger.LogInformation("DeleteTaskCommand.{methodName}. Начало удаления задачи. TaskId: {taskId}",
                methodName, request.Id);

            await taskService.DeleteAsync(request.Id, cancellationToken);

            logger.LogInformation("DeleteTaskCommand.{methodName}. Задача удалена. TaskId: {taskId}",
                methodName, request.Id);

            return Unit.Value;
        }
    }
}