namespace Umbrella.Domain.Entities;

/// <summary>
/// Пользователь системы
/// </summary>
public sealed class User : Entity
{
    /// <summary>
    /// Идентификатор пользователя в Keycloak
    /// </summary>
    public string KeycloakId { get; set; } = string.Empty;

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Email пользователя
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Имя пользователя (username)
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Дата первого входа
    /// </summary>
    public DateTime? FirstLoginAt { get; set; }

    /// <summary>
    /// Дата последнего входа
    /// </summary>
    public DateTime? LastLoginAt { get; set; }
}