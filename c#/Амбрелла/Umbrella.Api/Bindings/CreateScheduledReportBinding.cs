namespace Umbrella.Api.Bindings;

/// <summary>
/// Модель для создания запланированного отчета
/// </summary>
public sealed record CreateScheduledReportBinding
{
    /// <summary>
    /// Идентификатор отчета
    /// </summary>
    public Guid ReportId { get; init; }

    /// <summary>
    /// Идентификатор источника данных
    /// </summary>
    public Guid? DataSourceId { get; init; }

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
    public bool IsActive { get; init; } = true;
}