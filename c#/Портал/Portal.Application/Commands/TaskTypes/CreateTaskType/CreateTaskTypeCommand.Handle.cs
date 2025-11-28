using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Portal.Application.Contracts.Services;
using Portal.Domain.Dtos;
using Portal.Domain.Models;

namespace Portal.Application.Commands.TaskTypes.CreateTaskType;

public sealed partial class CreateTaskTypeCommand
{
    public sealed class CreateTaskTypeCommandHandle(
        ITaskTypeService taskTypeService,
        ILogger<CreateTaskTypeCommandHandle> logger) : IRequestHandler<CreateTaskTypeCommand, TaskTypeDto>
    {
        public async Task<TaskTypeDto> Handle(CreateTaskTypeCommand request, CancellationToken cancellationToken)
        {
            var methodName = nameof(Handle);
            logger.LogInformation("CreateTaskTypeCommand.{methodName}. Начало создания типа задачи. Name: {name}",
                methodName, request.Name);

            var model = request.Adapt<TaskTypeModel>();

            var result = await taskTypeService.CreateAsync(model, cancellationToken);

            logger.LogInformation("CreateTaskTypeCommand.{methodName}. Тип задачи создан. TaskTypeId: {taskTypeId}",
                methodName, result.Id);

            return result;
        }
    }
}