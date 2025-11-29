using MediatR;
using Umbrella.Domain.Dtos;

namespace Umbrella.Application.Commands.Users.UpdateUser;

/// <summary>
/// Команда для обновления пользователя
/// </summary>
public sealed partial class UpdateUserCommand(
    Guid id,
    string name,
    string email,
    string username)
    : IRequest<UserDto>
{
    public Guid Id { get; } = id;
    public string Name { get; } = name;
    public string Email { get; } = email;
    public string Username { get; } = username;
}