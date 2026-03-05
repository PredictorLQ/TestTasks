using System.Text.Json.Serialization;

namespace task.Models;

/// <summary>
/// DTO корневого объекта с городами, полученными из внешнего API.
/// </summary>
public class CityDto
{
    /// <summary>
    /// Коллекция данных по городам.
    /// </summary>
    [JsonPropertyName("city")]
    public List<CityDataDto>? Cities { get; set; }
}