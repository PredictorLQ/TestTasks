using TaskEntity = Portal.Domain.Entities.Task;

namespace Portal.Application.Contracts.Repositories;

/// <summary>
/// Интерфейс репозитория для работы с задачами
/// </summary>
public interface ITaskRepository
{
    /// <summary>
    /// Получить задачу по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор задачи</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Задача или null, если не найдена</returns>
    Task<TaskEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить все задачи
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Список всех задач</returns>
    Task<List<TaskEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Создать новую задачу
    /// </summary>
    /// <param name="task">Задача для создания</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Созданная задача</returns>
    Task<TaskEntity> CreateAsync(TaskEntity task, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить существующую задачу
    /// </summary>
    /// <param name="task">Задача для обновления</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Обновленная задача</returns>
    Task<TaskEntity> UpdateAsync(TaskEntity task, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить задачу (мягкое удаление)
    /// </summary>
    /// <param name="id">Идентификатор задачи для удаления</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    System.Threading.Tasks.Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Проверить существование задачи
    /// </summary>
    /// <param name="id">Идентификатор задачи</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>True, если задача существует, иначе False</returns>
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}