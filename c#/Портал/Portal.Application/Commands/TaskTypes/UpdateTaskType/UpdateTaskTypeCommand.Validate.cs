using FluentValidation;

namespace Portal.Application.Commands.TaskTypes.UpdateTaskType;

public sealed partial class UpdateTaskTypeCommand
{
    public sealed class UpdateTaskTypeCommandValidate : AbstractValidator<UpdateTaskTypeCommand>
    {
        public UpdateTaskTypeCommandValidate()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Идентификатор типа задачи обязателен");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Название типа задачи обязательно")
                .MaximumLength(100)
                .WithMessage("Название типа задачи не должно превышать 100 символов");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Описание типа задачи не должно превышать 500 символов")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}