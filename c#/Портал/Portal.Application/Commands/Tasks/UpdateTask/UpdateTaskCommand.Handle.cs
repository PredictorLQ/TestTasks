using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Portal.Application.Contracts.Services;
using Portal.Domain.Dtos;
using Portal.Domain.Models;

namespace Portal.Application.Commands.Tasks.UpdateTask;

public sealed partial class UpdateTaskCommand
{
    public sealed class UpdateTaskCommandHandle(
        ITaskService taskService,
        ILogger<UpdateTaskCommandHandle> logger) : IRequestHandler<UpdateTaskCommand, TaskDto>
    {
        public async Task<TaskDto> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var methodName = nameof(Handle);
            logger.LogInformation("UpdateTaskCommand.{methodName}. Начало обновления задачи. TaskId: {taskId}",
                methodName, request.Id);

            var model = request.Adapt<TaskModel>();

            var result = await taskService.UpdateAsync(request.Id, model, cancellationToken);

            logger.LogInformation("UpdateTaskCommand.{methodName}. Задача обновлена. TaskId: {taskId}",
                methodName, result.Id);

            return result;
        }
    }
}