using Portal.Domain.Dtos;
using Portal.Domain.Models;

namespace Portal.Application.Contracts.Services;

/// <summary>
/// Интерфейс сервиса для работы с задачами
/// </summary>
public interface ITaskService
{
    /// <summary>
    /// Получить задачу по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор задачи</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>DTO задачи или null, если не найдена</returns>
    Task<TaskDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить все задачи
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Список DTO всех задач</returns>
    Task<List<TaskDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Создать новую задачу
    /// </summary>
    /// <param name="model">Модель для создания задачи</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>DTO созданной задачи</returns>
    Task<TaskDto> CreateAsync(TaskModel model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить существующую задачу
    /// </summary>
    /// <param name="id">Идентификатор задачи для обновления</param>
    /// <param name="model">Модель с обновленными данными задачи</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>DTO обновленной задачи</returns>
    Task<TaskDto> UpdateAsync(Guid id, TaskModel model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить задачу (мягкое удаление)
    /// </summary>
    /// <param name="id">Идентификатор задачи для удаления</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}