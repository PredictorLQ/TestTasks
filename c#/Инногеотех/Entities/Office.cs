namespace task.Entities;

/// <summary>
/// Офис (пункт выдачи, постамат или склад).
/// </summary>
public class Office
{
    /// <summary>
    /// Уникальный идентификатор офиса.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Внутренний код офиса.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Код города, к которому относится офис.
    /// </summary>
    public int CityCode { get; set; }

    /// <summary>
    /// Уникальный идентификатор офиса (UUID).
    /// </summary>
    public string? Uuid { get; set; }

    /// <summary>
    /// Тип офиса.
    /// </summary>
    public OfficeType? Type { get; set; }

    /// <summary>
    /// Код страны офиса.
    /// </summary>
    public string CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// Географические координаты офиса.
    /// </summary>
    public Coordinates Coordinates { get; set; } = null!;

    /// <summary>
    /// Регион адреса офиса.
    /// </summary>
    public string? AddressRegion { get; set; }

    /// <summary>
    /// Город адреса офиса.
    /// </summary>
    public string? AddressCity { get; set; }

    /// <summary>
    /// Улица адреса офиса.
    /// </summary>
    public string? AddressStreet { get; set; }

    /// <summary>
    /// Номер дома адреса офиса.
    /// </summary>
    public string? AddressHouseNumber { get; set; }

    /// <summary>
    /// Номер квартиры/помещения офиса.
    /// </summary>
    public int? AddressApartment { get; set; }

    /// <summary>
    /// Рабочее время офиса.
    /// </summary>
    public string WorkTime { get; set; } = string.Empty;

    /// <summary>
    /// Коллекция телефонных номеров офиса.
    /// </summary>
    public ICollection<Phone> Phones { get; set; } = [];
}