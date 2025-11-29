using FluentValidation;

namespace Umbrella.Application.Commands.Reports.DeleteReport;

public sealed partial class DeleteReportCommand
{
    public sealed class DeleteReportCommandValidate : AbstractValidator<DeleteReportCommand>
    {
        public DeleteReportCommandValidate()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Идентификатор отчета обязателен");
        }
    }
}