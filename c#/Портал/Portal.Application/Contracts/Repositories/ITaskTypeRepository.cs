using Portal.Domain.Entities;

namespace Portal.Application.Contracts.Repositories;

/// <summary>
/// Интерфейс репозитория для работы с типами задач
/// </summary>
public interface ITaskTypeRepository
{
    /// <summary>
    /// Получить тип задачи по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор типа задачи</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Тип задачи или null, если не найден</returns>
    Task<TaskType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить все типы задач
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Список всех типов задач</returns>
    Task<List<TaskType>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Проверить существование типа задачи
    /// </summary>
    /// <param name="id">Идентификатор типа задачи</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>True, если тип задачи существует, иначе False</returns>
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Создать новый тип задачи
    /// </summary>
    /// <param name="taskType">Тип задачи для создания</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Созданный тип задачи</returns>
    Task<TaskType> CreateAsync(TaskType taskType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить существующий тип задачи
    /// </summary>
    /// <param name="taskType">Тип задачи для обновления</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Обновленный тип задачи</returns>
    Task<TaskType> UpdateAsync(TaskType taskType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить тип задачи (мягкое удаление)
    /// </summary>
    /// <param name="id">Идентификатор типа задачи для удаления</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    System.Threading.Tasks.Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}