using Umbrella.Domain.Entities;

namespace Umbrella.Application.Contracts.Repositories;

/// <summary>
/// Интерфейс репозитория для работы с запланированными отчетами
/// </summary>
public interface IScheduledReportRepository
{
    /// <summary>
    /// Получить запланированный отчет по идентификатору
    /// </summary>
    Task<ScheduledReport?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить все запланированные отчеты
    /// </summary>
    Task<List<ScheduledReport>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Создать новый запланированный отчет
    /// </summary>
    Task<ScheduledReport> CreateAsync(ScheduledReport scheduledReport, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить запланированный отчет
    /// </summary>
    Task<ScheduledReport> UpdateAsync(ScheduledReport scheduledReport, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить запланированный отчет (мягкое удаление)
    /// </summary>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Проверить существование запланированного отчета
    /// </summary>
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}