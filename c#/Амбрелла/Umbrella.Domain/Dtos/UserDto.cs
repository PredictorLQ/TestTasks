namespace Umbrella.Domain.Dtos;

/// <summary>
/// DTO пользователя
/// </summary>
public sealed record UserDto
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Идентификатор пользователя в Keycloak
    /// </summary>
    public string KeycloakId { get; init; } = string.Empty;

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Email пользователя
    /// </summary>
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// Имя пользователя (username)
    /// </summary>
    public string Username { get; init; } = string.Empty;

    /// <summary>
    /// Дата первого входа
    /// </summary>
    public DateTime? FirstLoginAt { get; init; }

    /// <summary>
    /// Дата последнего входа
    /// </summary>
    public DateTime? LastLoginAt { get; init; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; init; }
}