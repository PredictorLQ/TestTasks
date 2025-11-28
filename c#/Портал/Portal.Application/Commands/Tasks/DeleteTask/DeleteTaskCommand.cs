using MediatR;

namespace Portal.Application.Commands.Tasks.DeleteTask;

/// <summary>
/// Команда для удаления задачи
/// </summary>
public sealed partial class DeleteTaskCommand(Guid id) : IRequest<Unit>
{
    public Guid Id { get; } = id;
}