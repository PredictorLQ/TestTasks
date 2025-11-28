using MediatR;
using Portal.Domain.Dtos;

namespace Portal.Application.Queries.TaskTypes.GetTaskTypeById;

/// <summary>
/// Запрос для получения типа задачи по идентификатору
/// </summary>
public sealed record GetTaskTypeByIdQuery(Guid Id) : IRequest<TaskTypeDto?>;

