using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Umbrella.Application.Contracts.Services;
using Umbrella.Domain.Dtos;
using Umbrella.Domain.Models;

namespace Umbrella.Application.Commands.Users.CreateUser;

public sealed partial class CreateUserCommand
{
    public sealed class CreateUserCommandHandle(
        IUserService userService,
        ILogger<CreateUserCommandHandle> logger) : IRequestHandler<CreateUserCommand, UserDto>
    {
        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var methodName = nameof(Handle);
            logger.LogInformation("CreateUserCommand.{methodName}. Начало создания пользователя. KeycloakId: {keycloakId}",
                methodName, request.KeycloakId);

            var model = request.Adapt<UserModel>();

            var result = await userService.CreateAsync(model, cancellationToken);

            logger.LogInformation("CreateUserCommand.{methodName}. Пользователь создан. UserId: {userId}",
                methodName, result.Id);

            return result;
        }
    }
}