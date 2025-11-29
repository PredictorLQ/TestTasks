using Umbrella.Domain.Dtos;
using Umbrella.Domain.Models;

namespace Umbrella.Application.Contracts.Services;

/// <summary>
/// Интерфейс сервиса для работы с пользователями
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Получить пользователя по идентификатору
    /// </summary>
    Task<UserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить пользователя по идентификатору Keycloak
    /// </summary>
    Task<UserDto?> GetByKeycloakIdAsync(string keycloakId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить всех пользователей
    /// </summary>
    Task<List<UserDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Создать нового пользователя
    /// </summary>
    Task<UserDto> CreateAsync(UserModel model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить пользователя
    /// </summary>
    Task<UserDto> UpdateAsync(Guid id, UserModel model, CancellationToken cancellationToken = default);
}