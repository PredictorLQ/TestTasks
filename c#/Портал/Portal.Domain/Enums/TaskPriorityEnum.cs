using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Portal.Domain.Enums;

/// <summary>
/// Приоритет задачи
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TaskPriorityEnum
{
    /// <summary>
    /// Низкий приоритет
    /// </summary>
    [EnumMember(Value = "Low")]
    Low = 0,

    /// <summary>
    /// Средний приоритет
    /// </summary>
    [EnumMember(Value = "Medium")]
    Medium = 1,

    /// <summary>
    /// Высокий приоритет
    /// </summary>
    [EnumMember(Value = "High")]
    High = 2,

    /// <summary>
    /// Критический приоритет
    /// </summary>
    [EnumMember(Value = "Critical")]
    Critical = 3
}