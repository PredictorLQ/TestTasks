using MediatR;
using Portal.Domain.Dtos;

namespace Portal.Application.Commands.TaskTypes.UpdateTaskType;

/// <summary>
/// Команда для обновления существующего типа задачи
/// </summary>
public sealed partial class UpdateTaskTypeCommand(
    Guid id,
    string name,
    string? description)
    : IRequest<TaskTypeDto>
{
    public Guid Id { get; } = id;
    public string Name { get; } = name;
    public string? Description { get; } = description;
}