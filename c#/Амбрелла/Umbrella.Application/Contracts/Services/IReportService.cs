using Umbrella.Domain.Dtos;
using Umbrella.Domain.Models;

namespace Umbrella.Application.Contracts.Services;

/// <summary>
/// Интерфейс сервиса для работы с отчетами
/// </summary>
public interface IReportService
{
    /// <summary>
    /// Получить отчет по идентификатору
    /// </summary>
    Task<ReportDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить содержимое отчета
    /// </summary>
    Task<byte[]?> GetContentByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить все отчеты
    /// </summary>
    Task<List<ReportDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Создать новый отчет
    /// </summary>
    Task<ReportDto> CreateAsync(ReportModel model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить отчет
    /// </summary>
    Task<ReportDto> UpdateAsync(Guid id, ReportModel model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить отчет (мягкое удаление)
    /// </summary>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Сгенерировать отчет
    /// </summary>
    Task<byte[]?> GenerateAsync(Guid id, string? parameters, CancellationToken cancellationToken = default);
}