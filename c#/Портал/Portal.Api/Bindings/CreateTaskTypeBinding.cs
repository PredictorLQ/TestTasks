namespace Portal.Api.Bindings;

/// <summary>
/// Модель для создания типа задачи через API
/// </summary>
public sealed class CreateTaskTypeBinding
{
    /// <summary>
    /// Название типа задачи
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание типа задачи
    /// </summary>
    public string? Description { get; set; }
}