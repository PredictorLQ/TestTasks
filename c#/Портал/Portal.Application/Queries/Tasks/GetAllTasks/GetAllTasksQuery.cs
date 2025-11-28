using MediatR;
using Portal.Domain.Dtos;

namespace Portal.Application.Queries.Tasks.GetAllTasks;

/// <summary>
/// Запрос для получения всех задач
/// </summary>
public sealed partial class GetAllTasksQuery() : IRequest<List<TaskDto>>
{
}