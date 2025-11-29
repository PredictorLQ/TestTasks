using Umbrella.Domain.Entities;

namespace Umbrella.Application.Contracts.Repositories;

/// <summary>
/// Интерфейс репозитория для работы с пользователями
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Получить пользователя по идентификатору
    /// </summary>
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить пользователя по идентификатору Keycloak
    /// </summary>
    Task<User?> GetByKeycloakIdAsync(string keycloakId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить всех пользователей
    /// </summary>
    Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Создать нового пользователя
    /// </summary>
    Task<User> CreateAsync(User user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить пользователя
    /// </summary>
    Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Проверить существование пользователя
    /// </summary>
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}