using FluentValidation;

namespace Umbrella.Application.Commands.Reports.CreateReport;

public sealed partial class CreateReportCommand
{
    public sealed class CreateReportCommandValidate : AbstractValidator<CreateReportCommand>
    {
        public CreateReportCommandValidate()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Название отчета обязательно")
                .MaximumLength(200)
                .WithMessage("Название отчета не должно превышать 200 символов");

            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Содержимое отчета обязательно");

            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .WithMessage("Описание не должно превышать 1000 символов")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}