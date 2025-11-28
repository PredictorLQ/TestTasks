using FluentValidation;

namespace Portal.Application.Commands.TaskTypes.CreateTaskType;

public sealed partial class CreateTaskTypeCommand
{
    public sealed class CreateTaskTypeCommandValidate : AbstractValidator<CreateTaskTypeCommand>
    {
        public CreateTaskTypeCommandValidate()
        {
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