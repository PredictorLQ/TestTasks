using System.Text.Json.Serialization;

namespace task.Models;

/// <summary>
/// DTO терминала (офиса/пункта выдачи), полученного из внешнего API.
/// </summary>
public class TerminalDto
{
    /// <summary>
    /// Идентификатор терминала во внешней системе.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Название терминала.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Краткий адрес терминала.
    /// </summary>
    [JsonPropertyName("address")]
    public string? Address { get; set; }

    /// <summary>
    /// Полный адрес терминала.
    /// </summary>
    [JsonPropertyName("fullAddress")]
    public string? FullAddress { get; set; }

    /// <summary>
    /// Широта терминала.
    /// </summary>
    [JsonPropertyName("latitude")]
    public string? Latitude { get; set; }

    /// <summary>
    /// Долгота терминала.
    /// </summary>
    [JsonPropertyName("longitude")]
    public string? Longitude { get; set; }

    /// <summary>
    /// Признак, что терминал является пунктом выдачи (ПВЗ).
    /// </summary>
    [JsonPropertyName("isPVZ")]
    public bool? IsPVZ { get; set; }

    /// <summary>
    /// Список телефонов терминала.
    /// </summary>
    [JsonPropertyName("phones")]
    public List<PhoneDto>? Phones { get; set; }

    /// <summary>
    /// Расписание работы терминала.
    /// </summary>
    [JsonPropertyName("calcSchedule")]
    public CalcScheduleDto? CalcSchedule { get; set; }
}

/// <summary>
/// DTO телефонного номера терминала.
/// </summary>
public class PhoneDto
{
    /// <summary>
    /// Номер телефона.
    /// </summary>
    [JsonPropertyName("number")]
    public string? Number { get; set; }

    /// <summary>
    /// Тип телефона (например, мобильный, стационарный).
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    /// Дополнительный комментарий к номеру.
    /// </summary>
    [JsonPropertyName("comment")]
    public string? Comment { get; set; }

    /// <summary>
    /// Признак, что номер является основным.
    /// </summary>
    [JsonPropertyName("primary")]
    public bool? Primary { get; set; }
}

/// <summary>
/// DTO расписания работы терминала.
/// </summary>
public class CalcScheduleDto
{
    /// <summary>
    /// Время отправки (отгрузки) грузов.
    /// </summary>
    [JsonPropertyName("derival")]
    public string? Derival { get; set; }

    /// <summary>
    /// Время прибытия грузов.
    /// </summary>
    [JsonPropertyName("arrival")]
    public string? Arrival { get; set; }
}