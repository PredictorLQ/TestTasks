using Portal.Domain.Dtos;
using Portal.Domain.Models;

namespace Portal.Application.Contracts.Services;

/// <summary>
/// Интерфейс сервиса для работы с типами задач
/// </summary>
public interface ITaskTypeService
{
    /// <summary>
    /// Получить тип задачи по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор типа задачи</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>DTO типа задачи или null, если не найден</returns>
    Task<TaskTypeDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить все типы задач
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Список DTO всех типов задач</returns>
    Task<List<TaskTypeDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Создать новый тип задачи
    /// </summary>
    /// <param name="model">Модель для создания типа задачи</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>DTO созданного типа задачи</returns>
    Task<TaskTypeDto> CreateAsync(TaskTypeModel model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить существующий тип задачи
    /// </summary>
    /// <param name="id">Идентификатор типа задачи</param>
    /// <param name="model">Модель для обновления типа задачи</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>DTO обновленного типа задачи</returns>
    Task<TaskTypeDto> UpdateAsync(Guid id, TaskTypeModel model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить тип задачи (мягкое удаление)
    /// </summary>
    /// <param name="id">Идентификатор типа задачи для удаления</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}