using MediatR;
using Portal.Domain.Dtos;

namespace Portal.Application.Commands.TaskTypes.CreateTaskType;

/// <summary>
/// Команда для создания нового типа задачи
/// </summary>
public sealed partial class CreateTaskTypeCommand(
    string name,
    string? description)
    : IRequest<TaskTypeDto>
{
    public string Name { get; } = name;
    public string? Description { get; } = description;
}