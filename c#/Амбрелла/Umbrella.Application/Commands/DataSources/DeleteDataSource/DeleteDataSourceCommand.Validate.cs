using FluentValidation;

namespace Umbrella.Application.Commands.DataSources.DeleteDataSource;

public sealed partial class DeleteDataSourceCommand
{
    public sealed class DeleteDataSourceCommandValidate : AbstractValidator<DeleteDataSourceCommand>
    {
        public DeleteDataSourceCommandValidate()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Идентификатор источника данных обязателен");
        }
    }
}