namespace Umbrella.Domain.Dtos;

/// <summary>
/// DTO отчета
/// </summary>
public sealed record ReportDto
{
    /// <summary>
    /// Идентификатор отчета
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Название отчета
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Описание отчета
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Идентификатор источника данных
    /// </summary>
    public Guid? DataSourceId { get; init; }

    /// <summary>
    /// Название источника данных
    /// </summary>
    public string? DataSourceName { get; init; }

    /// <summary>
    /// Активен ли отчет
    /// </summary>
    public bool IsActive { get; init; }

    /// <summary>
    /// Размер содержимого отчета в байтах
    /// </summary>
    public int ContentSize { get; init; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; init; }
}