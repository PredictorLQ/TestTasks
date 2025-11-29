using Umbrella.Domain.Entities;

namespace Umbrella.Application.Contracts.Repositories;

/// <summary>
/// Интерфейс репозитория для работы с источниками данных
/// </summary>
public interface IDataSourceRepository
{
    /// <summary>
    /// Получить источник данных по идентификатору
    /// </summary>
    Task<DataSource?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить все источники данных
    /// </summary>
    Task<List<DataSource>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Создать новый источник данных
    /// </summary>
    Task<DataSource> CreateAsync(DataSource dataSource, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить источник данных
    /// </summary>
    Task<DataSource> UpdateAsync(DataSource dataSource, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить источник данных (мягкое удаление)
    /// </summary>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Проверить существование источника данных
    /// </summary>
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}