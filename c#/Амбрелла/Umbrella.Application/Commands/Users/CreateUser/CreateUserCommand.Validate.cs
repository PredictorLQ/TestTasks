using FluentValidation;

namespace Umbrella.Application.Commands.Users.CreateUser;

public sealed partial class CreateUserCommand
{
    public sealed class CreateUserCommandValidate : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidate()
        {
            RuleFor(x => x.KeycloakId)
                .NotEmpty()
                .WithMessage("Идентификатор Keycloak обязателен");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Имя пользователя обязательно")
                .MaximumLength(200)
                .WithMessage("Имя пользователя не должно превышать 200 символов");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email обязателен")
                .EmailAddress()
                .WithMessage("Некорректный формат email")
                .MaximumLength(200)
                .WithMessage("Email не должен превышать 200 символов");

            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Имя пользователя (username) обязательно")
                .MaximumLength(100)
                .WithMessage("Имя пользователя не должно превышать 100 символов");
        }
    }
}