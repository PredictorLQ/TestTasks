using MediatR;
using Microsoft.Extensions.Logging;
using Umbrella.Application.Contracts.Services;
using Umbrella.Domain.Dtos;

namespace Umbrella.Application.Queries.Users.GetUserById;

public sealed partial class GetUserByIdQuery
{
    public sealed class GetUserByIdQueryHandle(
        IUserService userService,
        ILogger<GetUserByIdQueryHandle> logger) : IRequestHandler<GetUserByIdQuery, UserDto?>
    {
        public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var methodName = nameof(Handle);
            logger.LogDebug("GetUserByIdQuery.{methodName}. Получение пользователя. UserId: {userId}",
                methodName, request.Id);

            var result = await userService.GetByIdAsync(request.Id, cancellationToken);

            logger.LogDebug("GetUserByIdQuery.{methodName}. Пользователь {found}. UserId: {userId}",
                methodName, result != null ? "найден" : "не найден", request.Id);

            return result;
        }
    }
}