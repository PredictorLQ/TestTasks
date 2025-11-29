using Umbrella.Domain.Enums;

namespace Umbrella.Domain.Dtos;

/// <summary>
/// DTO источника данных
/// </summary>
public sealed record DataSourceDto
{
    /// <summary>
    /// Идентификатор источника данных
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Название источника данных
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Тип источника данных
    /// </summary>
    public DataSourceType Type { get; init; }

    /// <summary>
    /// Описание источника данных
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Активен ли источник данных
    /// </summary>
    public bool IsActive { get; init; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; init; }
}