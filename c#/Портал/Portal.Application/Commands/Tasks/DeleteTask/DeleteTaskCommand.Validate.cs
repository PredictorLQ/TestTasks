using FluentValidation;

namespace Portal.Application.Commands.Tasks.DeleteTask;

public sealed partial class DeleteTaskCommand
{
    public sealed class DeleteTaskCommandValidate : AbstractValidator<DeleteTaskCommand>
    {
        public DeleteTaskCommandValidate()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Идентификатор задачи обязателен");
        }
    }
}