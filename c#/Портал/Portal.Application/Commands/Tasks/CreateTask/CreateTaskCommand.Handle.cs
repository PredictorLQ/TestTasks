using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Portal.Application.Contracts.Services;
using Portal.Domain.Dtos;
using Portal.Domain.Models;

namespace Portal.Application.Commands.Tasks.CreateTask;

public sealed partial class CreateTaskCommand
{
    public sealed class CreateTaskCommandHandle(
        ITaskService taskService,
        ILogger<CreateTaskCommandHandle> logger) : IRequestHandler<CreateTaskCommand, TaskDto>
    {
        public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var methodName = nameof(Handle);
            logger.LogInformation("CreateTaskCommand.{methodName}. Начало создания задачи. TaskTypeId: {taskTypeId}",
                methodName, request.TaskTypeId);

            var model = request.Adapt<TaskModel>();

            var result = await taskService.CreateAsync(model, cancellationToken);

            logger.LogInformation("CreateTaskCommand.{methodName}. Задача создана. TaskId: {taskId}",
                methodName, result.Id);

            return result;
        }
    }
}