using MediatR;
using Umbrella.Domain.Dtos;

namespace Umbrella.Application.Queries.Users.GetUserById;

/// <summary>
/// Запрос для получения пользователя по идентификатору
/// </summary>
public sealed partial class GetUserByIdQuery(Guid id) : IRequest<UserDto?>
{
    public Guid Id { get; } = id;
}