namespace Umbrella.Domain.Entities;

/// <summary>
/// Запланированный отчет
/// </summary>
public sealed class ScheduledReport : Entity
{
    /// <summary>
    /// Идентификатор отчета
    /// </summary>
    public Guid ReportId { get; set; }

    /// <summary>
    /// Отчет
    /// </summary>
    public Report Report { get; set; } = null!;

    /// <summary>
    /// Идентификатор источника данных
    /// </summary>
    public Guid? DataSourceId { get; set; }

    /// <summary>
    /// Источник данных
    /// </summary>
    public DataSource? DataSource { get; set; }

    /// <summary>
    /// Расписание выполнения (Cron выражение)
    /// </summary>
    public string Schedule { get; set; } = string.Empty;

    /// <summary>
    /// Название задания
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание задания
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Параметры отчета (JSON)
    /// </summary>
    public string? Parameters { get; set; }

    /// <summary>
    /// Активно ли задание
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Дата последнего выполнения
    /// </summary>
    public DateTime? LastRunAt { get; set; }

    /// <summary>
    /// Дата следующего выполнения
    /// </summary>
    public DateTime? NextRunAt { get; set; }

    /// <summary>
    /// Результат последнего выполнения (JSON)
    /// </summary>
    public string? LastResult { get; set; }
}