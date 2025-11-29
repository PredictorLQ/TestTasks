using FluentValidation;

namespace Umbrella.Application.Commands.DataSources.CreateDataSource;

public sealed partial class CreateDataSourceCommand
{
    public sealed class CreateDataSourceCommandValidate : AbstractValidator<CreateDataSourceCommand>
    {
        public CreateDataSourceCommandValidate()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Название источника данных обязательно")
                .MaximumLength(200)
                .WithMessage("Название источника данных не должно превышать 200 символов");

            RuleFor(x => x.ConnectionString)
                .NotEmpty()
                .WithMessage("Строка подключения обязательна")
                .MaximumLength(1000)
                .WithMessage("Строка подключения не должна превышать 1000 символов");

            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .WithMessage("Описание не должно превышать 1000 символов")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}