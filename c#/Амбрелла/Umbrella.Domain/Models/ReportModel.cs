namespace Umbrella.Domain.Models;

/// <summary>
/// Модель для создания/обновления отчета
/// </summary>
public sealed record ReportModel
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
    /// Содержимое отчета (MRT файл)
    /// </summary>
    public byte[] Content { get; init; } = [];

    /// <summary>
    /// Идентификатор источника данных
    /// </summary>
    public Guid? DataSourceId { get; init; }

    /// <summary>
    /// Активен ли отчет
    /// </summary>
    public bool IsActive { get; init; } = true;
}