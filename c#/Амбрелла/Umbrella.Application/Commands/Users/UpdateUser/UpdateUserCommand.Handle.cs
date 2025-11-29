using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Umbrella.Application.Contracts.Services;
using Umbrella.Domain.Dtos;
using Umbrella.Domain.Models;

namespace Umbrella.Application.Commands.Users.UpdateUser;

public sealed partial class UpdateUserCommand
{
    public sealed class UpdateUserCommandHandle(
        IUserService userService,
        ILogger<UpdateUserCommandHandle> logger) : IRequestHandler<UpdateUserCommand, UserDto>
    {
        public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var methodName = nameof(Handle);
            logger.LogInformation("UpdateUserCommand.{methodName}. Начало обновления пользователя. UserId: {userId}",
                methodName, request.Id);

            var model = request.Adapt<UserModel>();

            var result = await userService.UpdateAsync(request.Id, model, cancellationToken);

            logger.LogInformation("UpdateUserCommand.{methodName}. Пользователь обновлен. UserId: {userId}",
                methodName, request.Id);

            return result;
        }
    }
}