using Portal.Domain.Enums;

namespace Portal.Api.Bindings;

/// <summary>
/// Модель для создания задачи через API
/// </summary>
public sealed class CreateTaskBinding
{
    /// <summary>
    /// Название задачи
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Описание задачи
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Статус задачи
    /// </summary>
    public TaskStatusEnum Status { get; set; } = TaskStatusEnum.Pending;

    /// <summary>
    /// Приоритет задачи
    /// </summary>
    public TaskPriorityEnum Priority { get; set; } = TaskPriorityEnum.Medium;

    /// <summary>
    /// Срок выполнения задачи
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Идентификатор типа задачи
    /// </summary>
    public Guid TaskTypeId { get; set; }
}