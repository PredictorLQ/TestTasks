using Umbrella.Domain.Enums;

namespace Umbrella.Api.Bindings;

/// <summary>
/// Модель для создания источника данных
/// </summary>
public sealed record CreateDataSourceBinding
{
    /// <summary>
    /// Название источника данных
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Тип источника данных
    /// </summary>
    public DataSourceType Type { get; init; }

    /// <summary>
    /// Строка подключения к базе данных
    /// </summary>
    public string ConnectionString { get; init; } = string.Empty;

    /// <summary>
    /// Описание источника данных
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Активен ли источник данных
    /// </summary>
    public bool IsActive { get; init; } = true;
}