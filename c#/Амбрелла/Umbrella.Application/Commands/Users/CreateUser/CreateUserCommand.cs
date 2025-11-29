using MediatR;
using Umbrella.Domain.Dtos;

namespace Umbrella.Application.Commands.Users.CreateUser;

/// <summary>
/// Команда для создания нового пользователя
/// </summary>
public sealed partial class CreateUserCommand(
    string keycloakId,
    string name,
    string email,
    string username)
    : IRequest<UserDto>
{
    public string KeycloakId { get; } = keycloakId;
    public string Name { get; } = name;
    public string Email { get; } = email;
    public string Username { get; } = username;
}