using FluentValidation;

namespace Umbrella.Application.Commands.ScheduledReports.CreateScheduledReport;

public sealed partial class CreateScheduledReportCommand
{
    public sealed class CreateScheduledReportCommandValidate : AbstractValidator<CreateScheduledReportCommand>
    {
        public CreateScheduledReportCommandValidate()
        {
            RuleFor(x => x.ReportId)
                .NotEmpty()
                .WithMessage("Идентификатор отчета обязателен");

            RuleFor(x => x.Schedule)
                .NotEmpty()
                .WithMessage("Расписание выполнения обязательно")
                .MaximumLength(100)
                .WithMessage("Расписание не должно превышать 100 символов");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Название задания обязательно")
                .MaximumLength(200)
                .WithMessage("Название задания не должно превышать 200 символов");

            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .WithMessage("Описание не должно превышать 1000 символов")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}