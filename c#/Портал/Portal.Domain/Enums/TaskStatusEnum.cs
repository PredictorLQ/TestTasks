using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Portal.Domain.Enums;

/// <summary>
/// Статус задачи
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TaskStatusEnum
{
    /// <summary>
    /// Ожидает выполнения
    /// </summary>
    [EnumMember(Value = "Pending")]
    Pending = 0,

    /// <summary>
    /// В процессе выполнения
    /// </summary>
    [EnumMember(Value = "InProgress")]
    InProgress = 1,

    /// <summary>
    /// Выполнена
    /// </summary>
    [EnumMember(Value = "Completed")]
    Completed = 2,

    /// <summary>
    /// Отменена
    /// </summary>
    [EnumMember(Value = "Cancelled")]
    Cancelled = 3
}