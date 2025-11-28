using FluentValidation;

namespace Portal.Application.Commands.TaskTypes.DeleteTaskType;

public sealed partial class DeleteTaskTypeCommand
{
    public sealed class DeleteTaskTypeCommandValidate : AbstractValidator<DeleteTaskTypeCommand>
    {
        public DeleteTaskTypeCommandValidate()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Идентификатор типа задачи обязателен");
        }
    }
}