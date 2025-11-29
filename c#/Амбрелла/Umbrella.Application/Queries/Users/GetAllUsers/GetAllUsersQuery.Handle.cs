using MediatR;
using Microsoft.Extensions.Logging;
using Umbrella.Application.Contracts.Services;
using Umbrella.Domain.Dtos;

namespace Umbrella.Application.Queries.Users.GetAllUsers;

public sealed partial class GetAllUsersQuery
{
    public sealed class GetAllUsersQueryHandle(
        IUserService userService,
        ILogger<GetAllUsersQueryHandle> logger) : IRequestHandler<GetAllUsersQuery, List<UserDto>>
    {
        public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var methodName = nameof(Handle);
            logger.LogDebug("GetAllUsersQuery.{methodName}. Получение всех пользователей", methodName);

            var result = await userService.GetAllAsync(cancellationToken);

            logger.LogDebug("GetAllUsersQuery.{methodName}. Получено пользователей: {count}",
                methodName, result.Count);

            return result;
        }
    }
}