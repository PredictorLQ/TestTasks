namespace Portal.Domain.Models;

/// <summary>
/// Модель для создания и обновления типа задачи
/// </summary>
public sealed class TaskTypeModel
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}