using MediatR;
using Portal.Domain.Dtos;
using Portal.Domain.Enums;

namespace Portal.Application.Commands.Tasks.UpdateTask;

/// <summary>
/// Команда для обновления существующей задачи
/// </summary>
public sealed partial class UpdateTaskCommand(
    Guid id,
    string title,
    string? description,
    TaskStatusEnum status,
    TaskPriorityEnum priority,
    DateTime? dueDate,
    Guid taskTypeId)
    : IRequest<TaskDto>
{
    public Guid Id { get; } = id;
    public string Title { get; } = title;
    public string? Description { get; } = description;
    public TaskStatusEnum Status { get; } = status;
    public TaskPriorityEnum Priority { get; } = priority;
    public DateTime? DueDate { get; } = dueDate;
    public Guid TaskTypeId { get; } = taskTypeId;
}