using Umbrella.Domain.Enums;

namespace Umbrella.Domain.Entities;

/// <summary>
/// Источник данных для отчетов
/// </summary>
public sealed class DataSource : Entity
{
    /// <summary>
    /// Название источника данных
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Тип источника данных
    /// </summary>
    public DataSourceType Type { get; set; }

    /// <summary>
    /// Строка подключения к базе данных
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Описание источника данных
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Активен ли источник данных
    /// </summary>
    public bool IsActive { get; set; } = true;
}