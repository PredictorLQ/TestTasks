namespace Portal.Domain.Entities;

/// <summary>
/// Сущность типа задачи
/// </summary>
public sealed class TaskType : Entity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<Task> Tasks { get; set; } = [];
}