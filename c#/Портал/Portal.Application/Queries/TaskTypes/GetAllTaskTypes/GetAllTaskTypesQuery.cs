using MediatR;
using Portal.Domain.Dtos;

namespace Portal.Application.Queries.TaskTypes.GetAllTaskTypes;

/// <summary>
/// Запрос для получения всех типов задач
/// </summary>
public sealed record GetAllTaskTypesQuery() : IRequest<List<TaskTypeDto>>;

