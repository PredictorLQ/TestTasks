namespace Umbrella.Domain.Entities;

/// <summary>
/// Отчет Stimulsoft
/// </summary>
public sealed class Report : Entity
{
    /// <summary>
    /// Название отчета
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание отчета
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Содержимое отчета (MRT файл)
    /// </summary>
    public byte[] Content { get; set; } = [];

    /// <summary>
    /// Идентификатор источника данных
    /// </summary>
    public Guid? DataSourceId { get; set; }

    /// <summary>
    /// Источник данных
    /// </summary>
    public DataSource? DataSource { get; set; }

    /// <summary>
    /// Активен ли отчет
    /// </summary>
    public bool IsActive { get; set; } = true;
}