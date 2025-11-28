using MediatR;
using Microsoft.AspNetCore.Mvc;
using Portal.Api.Bindings;
using Portal.Application.Commands.TaskTypes.CreateTaskType;
using Portal.Application.Commands.TaskTypes.DeleteTaskType;
using Portal.Application.Commands.TaskTypes.UpdateTaskType;
using Portal.Application.Queries.TaskTypes.GetAllTaskTypes;
using Portal.Application.Queries.TaskTypes.GetTaskTypeById;

namespace Portal.Api.Controllers;

/// <summary>
/// Контроллер для управления типами задач
/// </summary>
[ApiController]
[Route("api/[controller]")]
public sealed class TaskTypesController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Получить все типы задач
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции. Используется для отмены долгих операций.</param>
    /// <returns>Список всех активных (не удаленных) типов задач с полной информацией о каждом типе</returns>
    /// <remarks>
    /// Возвращает список всех типов задач, которые не были удалены (мягкое удаление).
    /// Каждый тип задачи содержит полную информацию: идентификатор, название, описание и метаданные.
    /// 
    /// Типы задач используются для категоризации задач в системе.
    /// </remarks>
    /// <response code="200">Успешно получен список типов задач. Возвращается массив объектов TaskTypeDto.</response>
    /// <example>
    /// <code>
    /// GET /api/TaskTypes
    /// </code>
    /// </example>
    [HttpGet]
    [ProducesResponseType(typeof(List<Domain.Dtos.TaskTypeDto>), StatusCodes.Status200OK)]
    public Task<List<Domain.Dtos.TaskTypeDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return mediator.Send(new GetAllTaskTypesQuery(), cancellationToken);
    }

    /// <summary>
    /// Получить тип задачи по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор типа задачи в формате GUID. Пример: 3fa85f64-5717-4562-b3fc-2c963f66afa6</param>
    /// <param name="cancellationToken">Токен отмены операции. Используется для отмены долгих операций.</param>
    /// <returns>Полная информация о типе задачи или null, если тип задачи не найден или был удален</returns>
    /// <remarks>
    /// Возвращает полную информацию о типе задачи по её идентификатору.
    /// Если тип задачи не существует или был удален (мягкое удаление), возвращается null и статус 404.
    /// </remarks>
    /// <response code="200">Тип задачи успешно найден и возвращен. Тело ответа содержит объект TaskTypeDto с полной информацией о типе задачи.</response>
    /// <response code="404">Тип задачи с указанным идентификатором не найден или был удален. Тело ответа пустое.</response>
    /// <example>
    /// <code>
    /// GET /api/TaskTypes/3fa85f64-5717-4562-b3fc-2c963f66afa6
    /// </code>
    /// </example>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Domain.Dtos.TaskTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<Domain.Dtos.TaskTypeDto?> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        return mediator.Send(new GetTaskTypeByIdQuery(id), cancellationToken);
    }

    /// <summary>
    /// Создать новый тип задачи
    /// </summary>
    /// <param name="binding">Данные для создания типа задачи. Обязательное поле: Name (название, макс. 100 символов). Опциональное: Description (описание, макс. 500 символов).</param>
    /// <param name="cancellationToken">Токен отмены операции. Используется для отмены долгих операций.</param>
    /// <returns>Созданный тип задачи с присвоенным идентификатором, датой создания и другими метаданными</returns>
    /// <remarks>
    /// Создает новый тип задачи в системе. Типы задач используются для категоризации задач.
    /// При создании автоматически присваивается уникальный идентификатор (GUID),
    /// устанавливается дата создания (CreatedAt), версия (Version = 0) и флаг удаления (IsDeleted = false).
    /// 
    /// Валидация:
    /// - Название типа задачи обязательно и не должно превышать 100 символов
    /// - Описание не должно превышать 500 символов (если указано)
    /// 
    /// Примеры типов задач: "Разработка", "Тестирование", "Документация", "Исправление ошибок", "Рефакторинг"
    /// </remarks>
    /// <response code="200">Тип задачи успешно создан. Тело ответа содержит объект TaskTypeDto с полной информацией о созданном типе задачи. В заголовке Location указывается URL для получения созданного типа задачи.</response>
    /// <response code="400">Некорректные данные запроса. Тело запроса не соответствует ожидаемому формату или отсутствуют обязательные поля.</response>
    /// <response code="422">Ошибка валидации данных. Возможные причины: название типа задачи пустое или превышает 100 символов, описание превышает 500 символов.</response>
    /// <example>
    /// <code>
    /// POST /api/TaskTypes
    /// Content-Type: application/json
    /// {
    ///   "name": "Разработка",
    ///   "description": "Задачи, связанные с разработкой программного обеспечения"
    /// }
    /// </code>
    /// </example>
    [HttpPost]
    [ProducesResponseType(typeof(Domain.Dtos.TaskTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public Task<Domain.Dtos.TaskTypeDto> CreateAsync([FromBody] CreateTaskTypeBinding binding, CancellationToken cancellationToken = default)
    {
        var command = new CreateTaskTypeCommand(binding.Name, binding.Description);
        return mediator.Send(command, cancellationToken);
    }

    /// <summary>
    /// Обновить тип задачи
    /// </summary>
    /// <param name="id">Идентификатор типа задачи для обновления в формате GUID. Пример: 3fa85f64-5717-4562-b3fc-2c963f66afa6</param>
    /// <param name="binding">Обновленные данные типа задачи. Обязательное поле: Name (название, макс. 100 символов). Опциональное: Description (описание, макс. 500 символов).</param>
    /// <param name="cancellationToken">Токен отмены операции. Используется для отмены долгих операций.</param>
    /// <returns>Обновленный тип задачи с актуальными данными и обновленной датой изменения (ChangedAt)</returns>
    /// <remarks>
    /// Обновляет существующий тип задачи. При обновлении автоматически обновляется дата изменения (ChangedAt) и увеличивается версия (Version).
    /// 
    /// Валидация:
    /// - Тип задачи с указанным идентификатором должен существовать и не быть удаленным
    /// - Название типа задачи обязательно и не должно превышать 100 символов
    /// - Описание не должно превышать 500 символов (если указано)
    /// 
    /// Примечание: Обновление типа задачи не влияет на уже существующие задачи, использующие этот тип.
    /// </remarks>
    /// <response code="200">Тип задачи успешно обновлен. Тело ответа содержит объект TaskTypeDto с обновленной информацией о типе задачи.</response>
    /// <response code="400">Некорректные данные запроса. Тело запроса не соответствует ожидаемому формату или отсутствуют обязательные поля.</response>
    /// <response code="404">Тип задачи с указанным идентификатором не найден или был удален. Обновление невозможно.</response>
    /// <response code="422">Ошибка валидации данных. Возможные причины: название типа задачи пустое или превышает 100 символов, описание превышает 500 символов.</response>
    /// <example>
    /// <code>
    /// PUT /api/TaskTypes/3fa85f64-5717-4562-b3fc-2c963f66afa6
    /// Content-Type: application/json
    /// {
    ///   "name": "Обновленное название типа",
    ///   "description": "Обновленное описание типа задачи"
    /// }
    /// </code>
    /// </example>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Domain.Dtos.TaskTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public Task<Domain.Dtos.TaskTypeDto> UpdateAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateTaskTypeBinding binding,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateTaskTypeCommand(id, binding.Name, binding.Description);

        return mediator.Send(command, cancellationToken);
    }

    /// <summary>
    /// Удалить тип задачи
    /// </summary>
    /// <param name="id">Идентификатор типа задачи для удаления в формате GUID. Пример: 3fa85f64-5717-4562-b3fc-2c963f66afa6</param>
    /// <param name="cancellationToken">Токен отмены операции. Используется для отмены долгих операций.</param>
    /// <returns>Результат операции удаления. Тело ответа пустое при успешном удалении.</returns>
    /// <remarks>
    /// Выполняет мягкое удаление типа задачи. Тип задачи не удаляется физически из базы данных,
    /// а помечается как удаленный (IsDeleted = true) с установкой даты удаления (DeletedAt).
    /// 
    /// После мягкого удаления:
    /// - Тип задачи не будет возвращаться в списках типов задач
    /// - Тип задачи не будет доступен для получения по идентификатору
    /// - Данные типа задачи сохраняются в базе данных для возможного восстановления
    /// - Существующие задачи, использующие этот тип, не будут затронуты
    /// 
    /// Если тип задачи уже был удален или не существует, возвращается статус 404.
    /// 
    /// Внимание: Удаление типа задачи не удаляет связанные с ним задачи. Задачи остаются в системе,
    /// но тип задачи для них будет недоступен для просмотра.
    /// </remarks>
    /// <response code="204">Тип задачи успешно удален (мягкое удаление). Тело ответа пустое.</response>
    /// <response code="404">Тип задачи с указанным идентификатором не найден или уже был удален. Удаление невозможно.</response>
    /// <example>
    /// <code>
    /// DELETE /api/TaskTypes/3fa85f64-5717-4562-b3fc-2c963f66afa6
    /// </code>
    /// </example>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        await mediator.Send(new DeleteTaskTypeCommand(id), cancellationToken);
        return NoContent();
    }
}