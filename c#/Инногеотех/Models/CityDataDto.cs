using System.Text.Json.Serialization;

namespace task.Models;

/// <summary>
/// DTO данных отдельного города.
/// </summary>
public class CityDataDto
{
    /// <summary>
    /// Идентификатор города во внешней системе.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Наименование города.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Код города.
    /// </summary>
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    /// <summary>
    /// Внутренний числовой идентификатор города.
    /// </summary>
    [JsonPropertyName("cityID")]
    public int? CityId { get; set; }

    /// <summary>
    /// Контейнер с терминалами, относящимися к городу.
    /// </summary>
    [JsonPropertyName("terminals")]
    public TerminalsContainerDto? Terminals { get; set; }
}