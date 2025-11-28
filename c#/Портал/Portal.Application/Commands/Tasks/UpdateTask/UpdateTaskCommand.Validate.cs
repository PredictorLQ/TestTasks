using FluentValidation;

namespace Portal.Application.Commands.Tasks.UpdateTask;

public sealed partial class UpdateTaskCommand
{
    public sealed class UpdateTaskCommandValidate : AbstractValidator<UpdateTaskCommand>
    {
        public UpdateTaskCommandValidate()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Идентификатор задачи обязателен");

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