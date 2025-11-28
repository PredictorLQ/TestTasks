using FluentValidation;

namespace Portal.Application.Commands.Tasks.CreateTask;

public sealed partial class CreateTaskCommand
{
    public sealed class CreateTaskCommandValidate : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidate()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Название задачи обязательно")
                .MaximumLength(200)
                .WithMessage("Название задачи не должно превышать 200 символов");

            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .WithMessage("Описание задачи не должно превышать 1000 символов")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.TaskTypeId)
                .NotEmpty()
                .WithMessage("Тип задачи обязателен");
        }
    }
}