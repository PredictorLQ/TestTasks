using MediatR;
using Portal.Domain.Dtos;

namespace Portal.Application.Queries.Tasks.GetTaskById;

/// <summary>
/// Запрос для получения задачи по идентификатору
/// </summary>
public sealed partial class GetTaskByIdQuery(Guid id) : IRequest<TaskDto?>
{
    public Guid Id { get; } = id;
}