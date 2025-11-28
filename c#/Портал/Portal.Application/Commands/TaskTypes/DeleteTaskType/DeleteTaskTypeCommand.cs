using MediatR;

namespace Portal.Application.Commands.TaskTypes.DeleteTaskType;

/// <summary>
/// Команда для удаления типа задачи
/// </summary>
public sealed partial class DeleteTaskTypeCommand(Guid id) : IRequest<Unit>
{
    public Guid Id { get; } = id;
}