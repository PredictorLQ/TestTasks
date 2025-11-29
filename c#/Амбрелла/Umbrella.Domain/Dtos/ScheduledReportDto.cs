namespace Umbrella.Domain.Dtos;

/// <summary>
/// DTO запланированного отчета
/// </summary>
public sealed record ScheduledReportDto
{
    /// <summary>
    /// Идентификатор запланированного отчета
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Идентификатор отчета
    /// </summary>
    public Guid ReportId { get; init; }

    /// <summary>
    /// Название отчета
    /// </summary>
    public string ReportName { get; init; } = string.Empty;

    /// <summary>
    /// Идентификатор источника данных
    /// </summary>
    public Guid? DataSourceId { get; init; }

    /// <summary>
    /// Название источника данных
    /// </summary>
    public string? DataSourceName { get; init; }

    /// <summary>
    /// Расписание выполнения (Cron выражение)
    /// </summary>
    public string Schedule { get; init; } = string.Empty;

    /// <summary>
    /// Название задания
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Описание задания
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Параметры отчета (JSON)
    /// </summary>
    public string? Parameters { get; init; }

    /// <summary>
    /// Активно ли задание
    /// </summary>
    public bool IsActive { get; init; }

    /// <summary>
    /// Дата последнего выполнения
    /// </summary>
    public DateTime? LastRunAt { get; init; }

    /// <summary>
    /// Дата следующего выполнения
    /// </summary>
    public DateTime? NextRunAt { get; init; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; init; }
}