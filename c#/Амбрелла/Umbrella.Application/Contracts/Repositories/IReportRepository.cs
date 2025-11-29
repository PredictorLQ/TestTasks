using Umbrella.Domain.Entities;

namespace Umbrella.Application.Contracts.Repositories;

/// <summary>
/// Интерфейс репозитория для работы с отчетами
/// </summary>
public interface IReportRepository
{
    /// <summary>
    /// Получить отчет по идентификатору
    /// </summary>
    Task<Report?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить все отчеты
    /// </summary>
    Task<List<Report>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Создать новый отчет
    /// </summary>
    Task<Report> CreateAsync(Report report, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить отчет
    /// </summary>
    Task<Report> UpdateAsync(Report report, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить отчет (мягкое удаление)
    /// </summary>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Проверить существование отчета
    /// </summary>
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}