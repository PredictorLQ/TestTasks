using Portal.Domain.Enums;

namespace Portal.Domain.Entities;

/// <summary>
/// Сущность задачи
/// </summary>
public sealed class Task : Entity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskStatusEnum Status { get; set; } = TaskStatusEnum.Pending;
    public TaskPriorityEnum Priority { get; set; } = TaskPriorityEnum.Medium;
    public DateTime? DueDate { get; set; }
    public Guid TaskTypeId { get; set; }
    public TaskType TaskType { get; set; } = null!;

    public override void CascadeDelete()
    {
        Delete();
    }
}