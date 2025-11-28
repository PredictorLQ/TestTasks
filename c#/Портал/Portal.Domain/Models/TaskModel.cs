using Portal.Domain.Enums;

namespace Portal.Domain.Models;

/// <summary>
/// Модель для создания и обновления задачи
/// </summary>
public sealed class TaskModel
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskStatusEnum Status { get; set; } = TaskStatusEnum.Pending;
    public TaskPriorityEnum Priority { get; set; } = TaskPriorityEnum.Medium;
    public DateTime? DueDate { get; set; }
    public Guid TaskTypeId { get; set; }
}