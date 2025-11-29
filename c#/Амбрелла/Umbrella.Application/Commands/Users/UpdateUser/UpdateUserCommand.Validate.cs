using FluentValidation;

namespace Umbrella.Application.Commands.Users.UpdateUser;

public sealed partial class UpdateUserCommand
{
    public sealed class UpdateUserCommandValidate : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidate()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Идентификатор пользователя обязателен");

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