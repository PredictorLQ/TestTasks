using FluentValidation;

namespace Umbrella.Application.Commands.Reports.UpdateReport;

public sealed partial class UpdateReportCommand
{
    public sealed class UpdateReportCommandValidate : AbstractValidator<UpdateReportCommand>
    {
        public UpdateReportCommandValidate()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Идентификатор отчета обязателен");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Название отчета обязательно")
                .MaximumLength(200)
                .WithMessage("Название отчета не должно превышать 200 символов");

            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .WithMessage("Описание не должно превышать 1000 символов")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}