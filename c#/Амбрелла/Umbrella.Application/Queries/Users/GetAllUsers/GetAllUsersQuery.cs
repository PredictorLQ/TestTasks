using MediatR;
using Umbrella.Domain.Dtos;

namespace Umbrella.Application.Queries.Users.GetAllUsers;

/// <summary>
/// Запрос для получения всех пользователей
/// </summary>
public sealed partial class GetAllUsersQuery() : IRequest<List<UserDto>>
{
}