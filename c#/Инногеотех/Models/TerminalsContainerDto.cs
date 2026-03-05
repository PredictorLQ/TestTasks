using System.Text.Json.Serialization;

namespace task.Models;

/// <summary>
/// Контейнер DTO для списка терминалов по городу.
/// </summary>
public class TerminalsContainerDto
{
    /// <summary>
    /// Список терминалов города.
    /// </summary>
    [JsonPropertyName("terminal")]
    public List<TerminalDto>? TerminalList { get; set; }
}