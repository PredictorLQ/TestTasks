namespace task.Entities;

/// <summary>
/// Телефонный номер, связанный с офисом.
/// </summary>
public class Phone
{
    /// <summary>
    /// Уникальный идентификатор телефона.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор связанного офиса.
    /// </summary>
    public int OfficeId { get; set; }

    /// <summary>
    /// Номер телефона.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Дополнительная информация по телефону.
    /// </summary>
    public string? Additional { get; set; }

    /// <summary>
    /// Навигационное свойство офиса.
    /// </summary>
    public Office Office { get; set; } = null!;
}