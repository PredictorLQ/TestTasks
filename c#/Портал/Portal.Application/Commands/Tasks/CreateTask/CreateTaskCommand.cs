using MediatR;
using Portal.Domain.Dtos;
using Portal.Domain.Enums;

namespace Portal.Application.Commands.Tasks.CreateTask;

/// <summary>
/// Команда для создания новой задачи
/// </summary>
public sealed partial class CreateTaskCommand(
    string title,
    string? description,
    TaskStatusEnum status,
    TaskPriorityEnum priority,
    DateTime? dueDate,
    Guid taskTypeId)
    : IRequest<TaskDto>
{
    public string Title { get; } = title;
    public string? Description { get; } = description;
    public TaskStatusEnum Status { get; } = status;
    public TaskPriorityEnum Priority { get; } = priority;
    public DateTime? DueDate { get; } = dueDate;
    public Guid TaskTypeId { get; } = taskTypeId;
}