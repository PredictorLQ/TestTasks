using Mapster;
using Microsoft.Extensions.Logging;
using Umbrella.Application.Contracts.Repositories;
using Umbrella.Application.Contracts.Services;
using Umbrella.Domain.Dtos;
using Umbrella.Domain.Models;

namespace Umbrella.Infrastructure.Services;

public sealed class UserService(
    IUserRepository userRepository,
    ILogger<UserService> logger) : IUserService
{
    private readonly string _serviceName = nameof(UserService);

    public async Task<UserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var methodName = nameof(GetByIdAsync);
        logger.LogDebug("{serviceName}.{methodName}. Получение пользователя. UserId: {userId}",
            _serviceName, methodName, id);

        var user = await userRepository.GetByIdAsync(id, cancellationToken);
        if (user == null)
        {
            logger.LogWarning("{serviceName}.{methodName}. Пользователь не найден. UserId: {userId}",
                _serviceName, methodName, id);
            return null;
        }

        return user.Adapt<UserDto>();
    }

    public async Task<UserDto?> GetByKeycloakIdAsync(string keycloakId, CancellationToken cancellationToken = default)
    {
        var methodName = nameof(GetByKeycloakIdAsync);
        logger.LogDebug("{serviceName}.{methodName}. Получение пользователя. KeycloakId: {keycloakId}",
            _serviceName, methodName, keycloakId);

        var user = await userRepository.GetByKeycloakIdAsync(keycloakId, cancellationToken);
        if (user == null)
        {
            logger.LogWarning("{serviceName}.{methodName}. Пользователь не найден. KeycloakId: {keycloakId}",
                _serviceName, methodName, keycloakId);
            return null;
        }

        return user.Adapt<UserDto>();
    }

    public async Task<List<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var methodName = nameof(GetAllAsync);
        logger.LogDebug("{serviceName}.{methodName}. Получение всех пользователей", _serviceName, methodName);

        var users = await userRepository.GetAllAsync(cancellationToken);
        return users.Adapt<List<UserDto>>();
    }

    public async Task<UserDto> CreateAsync(UserModel model, CancellationToken cancellationToken = default)
    {
        var methodName = nameof(CreateAsync);
        logger.LogInformation("{serviceName}.{methodName}. Создание пользователя. KeycloakId: {keycloakId}",
            _serviceName, methodName, model.KeycloakId);

        var user = model.Adapt<Domain.Entities.User>();
        user.FirstLoginAt = DateTime.UtcNow;

        var createdUser = await userRepository.CreateAsync(user, cancellationToken);

        logger.LogInformation("{serviceName}.{methodName}. Пользователь создан. UserId: {userId}",
            _serviceName, methodName, createdUser.Id);

        return createdUser.Adapt<UserDto>();
    }

    public async Task<UserDto> UpdateAsync(Guid id, UserModel model, CancellationToken cancellationToken = default)
    {
        var methodName = nameof(UpdateAsync);
        logger.LogInformation("{serviceName}.{methodName}. Обновление пользователя. UserId: {userId}",
            _serviceName, methodName, id);

        var user = await userRepository.GetByIdAsync(id, cancellationToken);
        if (user == null)
        {
            logger.LogError("{serviceName}.{methodName}. Пользователь не найден. UserId: {userId}",
                _serviceName, methodName, id);
            throw new InvalidOperationException($"Пользователь с Id {id} не найден");
        }

        user.Name = model.Name;
        user.Email = model.Email;
        user.Username = model.Username;
        user.LastLoginAt = DateTime.UtcNow;

        var updatedUser = await userRepository.UpdateAsync(user, cancellationToken);

        logger.LogInformation("{serviceName}.{methodName}. Пользователь обновлен. UserId: {userId}",
            _serviceName, methodName, updatedUser.Id);

        return updatedUser.Adapt<UserDto>();
    }
}