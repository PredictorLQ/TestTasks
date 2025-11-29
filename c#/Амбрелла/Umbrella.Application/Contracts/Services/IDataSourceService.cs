using Umbrella.Domain.Dtos;
using Umbrella.Domain.Models;

namespace Umbrella.Application.Contracts.Services;

/// <summary>
/// Интерфейс сервиса для работы с источниками данных
/// </summary>
public interface IDataSourceService
{
    /// <summary>
    /// Получить источник данных по идентификатору
    /// </summary>
    Task<DataSourceDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить все источники данных
    /// </summary>
    Task<List<DataSourceDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Создать новый источник данных
    /// </summary>
    Task<DataSourceDto> CreateAsync(DataSourceModel model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить источник данных
    /// </summary>
    Task<DataSourceDto> UpdateAsync(Guid id, DataSourceModel model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить источник данных (мягкое удаление)
    /// </summary>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}