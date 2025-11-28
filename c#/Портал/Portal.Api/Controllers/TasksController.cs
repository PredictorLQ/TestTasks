using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Portal.Api.Bindings;
using Portal.Application.Commands.Tasks.CreateTask;
using Portal.Application.Commands.Tasks.DeleteTask;
using Portal.Application.Commands.Tasks.UpdateTask;
using Portal.Application.Queries.Tasks.GetAllTasks;
using Portal.Application.Queries.Tasks.GetTaskById;

namespace Portal.Api.Controllers;

/// <summary>
/// Контроллер для управления задачами
/// </summary>
/// <remarks>
/// Предоставляет полный набор операций CRUD для работы с задачами:
/// - Получение списка всех задач
/// - Получение задачи по идентификатору
/// - Создание новой задачи
/// - Обновление существующей задачи
/// - Удаление задачи (мягкое удаление)
/// </remarks>
[ApiController]
[Route("api/[controller]")]
public sealed class TasksController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Получить все задачи
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции. Используется для отмены долгих операций.</param>
    /// <returns>Список всех активных (не удаленных) задач с полной информацией о каждой задаче</returns>
    /// <remarks>
    /// Возвращает список всех задач, которые не были удалены (мягкое удаление).
    /// Каждая задача содержит полную информацию: идентификатор, название, описание, статус, приоритет, срок выполнения, тип задачи и метаданные.
    /// </remarks>
    /// <response code="200">Успешно получен список задач. Возвращается массив объектов TaskDto.</response>
    /// <example>
    /// <code>
    /// GET /api/Tasks
    /// </code>
    /// </example>
    [HttpGet]
    [ProducesResponseType(typeof(List<Domain.Dtos.TaskDto>), StatusCodes.Status200OK)]
    public Task<List<Domain.Dtos.TaskDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return mediator.Send(new GetAllTasksQuery(), cancellationToken);
    }

    /// <summary>
    /// Получить задачу по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор задачи в формате GUID. Пример: 3fa85f64-5717-4562-b3fc-2c963f66afa6</param>
    /// <param name="cancellationToken">Токен отмены операции. Используется для отмены долгих операций.</param>
    /// <returns>Полная информация о задаче или null, если задача не найдена или была удалена</returns>
    /// <remarks>
    /// Возвращает полную информацию о задаче по её идентификатору.
    /// Если задача не существует или была удалена (мягкое удаление), возвращается null и статус 404.
    /// </remarks>
    /// <response code="200">Задача успешно найдена и возвращена. Тело ответа содержит объект TaskDto с полной информацией о задаче.</response>
    /// <response code="404">Задача с указанным идентификатором не найдена или была удалена. Тело ответа пустое.</response>
    /// <example>
    /// <code>
    /// GET /api/Tasks/3fa85f64-5717-4562-b3fc-2c963f66afa6
    /// </code>
    /// </example>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Domain.Dtos.TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<Domain.Dtos.TaskDto?> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        return mediator.Send(new GetTaskByIdQuery(id), cancellationToken);
    }

    /// <summary>
    /// Создать новую задачу
    /// </summary>
    /// <param name="binding">Данные для создания задачи. Обязательные поля: Title (название, макс. 200 символов), Status (статус), Priority (приоритет), TaskTypeId (идентификатор типа задачи). Опциональные: Description (описание, макс. 1000 символов), DueDate (срок выполнения).</param>
    /// <param name="cancellationToken">Токен отмены операции. Используется для отмены долгих операций.</param>
    /// <returns>Созданная задача с присвоенным идентификатором, датой создания и другими метаданными</returns>
    /// <remarks>
    /// Создает новую задачу в системе. При создании автоматически присваивается уникальный идентификатор (GUID),
    /// устанавливается дата создания (CreatedAt), версия (Version = 0) и флаг удаления (IsDeleted = false).
    /// 
    /// Валидация:
    /// - Название задачи обязательно и не должно превышать 200 символов
    /// - Описание не должно превышать 1000 символов (если указано)
    /// - Тип задачи (TaskTypeId) должен существовать в системе
    /// 
    /// Статусы задачи: Pending (Ожидает выполнения), InProgress (В процессе), Completed (Выполнена), Cancelled (Отменена)
    /// Приоритеты задачи: Low (Низкий), Medium (Средний), High (Высокий), Critical (Критический)
    /// </remarks>
    /// <response code="201">Задача успешно создана. Тело ответа содержит объект TaskDto с полной информацией о созданной задаче. В заголовке Location указывается URL для получения созданной задачи.</response>
    /// <response code="400">Некорректные данные запроса. Тело запроса не соответствует ожидаемому формату или отсутствуют обязательные поля.</response>
    /// <response code="422">Ошибка валидации данных. Возможные причины: название задачи пустое или превышает 200 символов, описание превышает 1000 символов, тип задачи с указанным TaskTypeId не существует в системе.</response>
    /// <example>
    /// <code>
    /// POST /api/Tasks
    /// Content-Type: application/json
    /// {
    ///   "title": "Реализовать новую функцию",
    ///   "description": "Добавить возможность экспорта данных",
    ///   "status": "Pending",
    ///   "priority": "High",
    ///   "dueDate": "2024-12-31T23:59:59Z",
    ///   "taskTypeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    /// }
    /// </code>
    /// </example>
    [HttpPost]
    [ProducesResponseType(typeof(Domain.Dtos.TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public Task<Domain.Dtos.TaskDto> CreateAsync([FromBody] CreateTaskBinding binding, CancellationToken cancellationToken = default)
    {
        var command = binding.Adapt<CreateTaskCommand>();
        return mediator.Send(command, cancellationToken);
    }

    /// <summary>
    /// Обновить задачу
    /// </summary>
    /// <param name="id">Идентификатор задачи для обновления в формате GUID. Пример: 3fa85f64-5717-4562-b3fc-2c963f66afa6</param>
    /// <param name="binding">Обновленные данные задачи. Все поля обязательны: Title (название, макс. 200 символов), Status (статус), Priority (приоритет), TaskTypeId (идентификатор типа задачи). Опциональные: Description (описание, макс. 1000 символов), DueDate (срок выполнения).</param>
    /// <param name="cancellationToken">Токен отмены операции. Используется для отмены долгих операций.</param>
    /// <returns>Обновленная задача с актуальными данными и обновленной датой изменения (ChangedAt)</returns>
    /// <remarks>
    /// Обновляет существующую задачу. При обновлении автоматически обновляется дата изменения (ChangedAt) и увеличивается версия (Version).
    /// 
    /// Валидация:
    /// - Задача с указанным идентификатором должна существовать и не быть удаленной
    /// - Название задачи обязательно и не должно превышать 200 символов
    /// - Описание не должно превышать 1000 символов (если указано)
    /// - Тип задачи (TaskTypeId) должен существовать в системе
    /// 
    /// Статусы задачи: Pending (Ожидает выполнения), InProgress (В процессе), Completed (Выполнена), Cancelled (Отменена)
    /// Приоритеты задачи: Low (Низкий), Medium (Средний), High (Высокий), Critical (Критический)
    /// </remarks>
    /// <response code="200">Задача успешно обновлена. Тело ответа содержит объект TaskDto с обновленной информацией о задаче.</response>
    /// <response code="400">Некорректные данные запроса. Тело запроса не соответствует ожидаемому формату или отсутствуют обязательные поля.</response>
    /// <response code="404">Задача с указанным идентификатором не найдена или была удалена. Обновление невозможно.</response>
    /// <response code="422">Ошибка валидации данных. Возможные причины: название задачи пустое или превышает 200 символов, описание превышает 1000 символов, тип задачи с указанным TaskTypeId не существует в системе.</response>
    /// <example>
    /// <code>
    /// PUT /api/Tasks/3fa85f64-5717-4562-b3fc-2c963f66afa6
    /// Content-Type: application/json
    /// {
    ///   "title": "Обновленное название задачи",
    ///   "description": "Обновленное описание",
    ///   "status": "InProgress",
    ///   "priority": "Medium",
    ///   "dueDate": "2024-12-25T23:59:59Z",
    ///   "taskTypeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    /// }
    /// </code>
    /// </example>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Domain.Dtos.TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public Task<Domain.Dtos.TaskDto> UpdateAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateTaskBinding binding,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateTaskCommand(
            id,
            binding.Title,
            binding.Description,
            binding.Status,
            binding.Priority,
            binding.DueDate,
            binding.TaskTypeId);

        return mediator.Send(command, cancellationToken);
    }

    /// <summary>
    /// Удалить задачу
    /// </summary>
    /// <param name="id">Идентификатор задачи для удаления в формате GUID. Пример: 3fa85f64-5717-4562-b3fc-2c963f66afa6</param>
    /// <param name="cancellationToken">Токен отмены операции. Используется для отмены долгих операций.</param>
    /// <returns>Результат операции удаления. Тело ответа пустое при успешном удалении.</returns>
    /// <remarks>
    /// Выполняет мягкое удаление задачи. Задача не удаляется физически из базы данных,
    /// а помечается как удаленная (IsDeleted = true) с установкой даты удаления (DeletedAt).
    /// 
    /// После мягкого удаления:
    /// - Задача не будет возвращаться в списках задач
    /// - Задача не будет доступна для получения по идентификатору
    /// - Данные задачи сохраняются в базе данных для возможного восстановления
    /// 
    /// Если задача уже была удалена или не существует, возвращается статус 404.
    /// </remarks>
    /// <response code="204">Задача успешно удалена (мягкое удаление). Тело ответа пустое.</response>
    /// <response code="404">Задача с указанным идентификатором не найдена или уже была удалена. Удаление невозможно.</response>
    /// <example>
    /// <code>
    /// DELETE /api/Tasks/3fa85f64-5717-4562-b3fc-2c963f66afa6
    /// </code>
    /// </example>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        await mediator.Send(new DeleteTaskCommand(id), cancellationToken);
        return NoContent();
    }
}