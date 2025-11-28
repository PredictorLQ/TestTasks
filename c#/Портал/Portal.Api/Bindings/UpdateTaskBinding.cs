using Portal.Domain.Enums;

namespace Portal.Api.Bindings;

/// <summary>
/// Модель для обновления задачи через API
/// </summary>
public sealed class UpdateTaskBinding
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
    public TaskStatusEnum Status { get; set; }

    /// <summary>
    /// Приоритет задачи
    /// </summary>
    public TaskPriorityEnum Priority { get; set; }

    /// <summary>
    /// Срок выполнения задачи
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Идентификатор типа задачи
    /// </summary>
    public Guid TaskTypeId { get; set; }
}