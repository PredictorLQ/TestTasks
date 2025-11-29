using Umbrella.Domain.Dtos;
using Umbrella.Domain.Models;

namespace Umbrella.Application.Contracts.Services;

/// <summary>
/// Интерфейс сервиса для работы с запланированными отчетами
/// </summary>
public interface IScheduledReportService
{
    /// <summary>
    /// Получить запланированный отчет по идентификатору
    /// </summary>
    Task<ScheduledReportDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить все запланированные отчеты
    /// </summary>
    Task<List<ScheduledReportDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Создать новый запланированный отчет
    /// </summary>
    Task<ScheduledReportDto> CreateAsync(ScheduledReportModel model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить запланированный отчет
    /// </summary>
    Task<ScheduledReportDto> UpdateAsync(Guid id, ScheduledReportModel model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить запланированный отчет (мягкое удаление)
    /// </summary>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}