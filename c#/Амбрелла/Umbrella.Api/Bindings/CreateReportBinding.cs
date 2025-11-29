namespace Umbrella.Api.Bindings;

/// <summary>
/// Модель для создания отчета
/// </summary>
public sealed record CreateReportBinding
{
    /// <summary>
    /// Название отчета
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Описание отчета
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Содержимое отчета (MRT файл в Base64)
    /// </summary>
    public string ContentBase64 { get; init; } = string.Empty;

    /// <summary>
    /// Идентификатор источника данных
    /// </summary>
    public Guid? DataSourceId { get; init; }

    /// <summary>
    /// Активен ли отчет
    /// </summary>
    public bool IsActive { get; init; } = true;
}