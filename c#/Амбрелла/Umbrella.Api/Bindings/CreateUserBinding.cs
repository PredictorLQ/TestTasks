namespace Umbrella.Api.Bindings;

/// <summary>
/// Модель для создания пользователя
/// </summary>
public sealed record CreateUserBinding
{
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
}