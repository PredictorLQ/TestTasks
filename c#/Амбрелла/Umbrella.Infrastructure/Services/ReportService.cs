using Mapster;
using Microsoft.Extensions.Logging;
using Umbrella.Application.Contracts.Repositories;
using Umbrella.Application.Contracts.Services;
using Umbrella.Domain.Dtos;
using Umbrella.Domain.Models;

namespace Umbrella.Infrastructure.Services;

public sealed class ReportService(
    IReportRepository reportRepository,
    ILogger<ReportService> logger) : IReportService
{
    private readonly string _serviceName = nameof(ReportService);

    public async Task<ReportDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var methodName = nameof(GetByIdAsync);
        logger.LogDebug("{serviceName}.{methodName}. Получение отчета. ReportId: {reportId}",
            _serviceName, methodName, id);

        var report = await reportRepository.GetByIdAsync(id, cancellationToken);
        if (report == null)
        {
            logger.LogWarning("{serviceName}.{methodName}. Отчет не найден. ReportId: {reportId}",
                _serviceName, methodName, id);
            return null;
        }

        var dto = report.Adapt<ReportDto>();
        return dto with { ContentSize = report.Content.Length, DataSourceName = report.DataSource?.Name };
    }

    public async Task<byte[]?> GetContentByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var methodName = nameof(GetContentByIdAsync);
        logger.LogDebug("{serviceName}.{methodName}. Получение содержимого отчета. ReportId: {reportId}",
            _serviceName, methodName, id);

        var report = await reportRepository.GetByIdAsync(id, cancellationToken);
        return report?.Content;
    }

    public async Task<List<ReportDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var methodName = nameof(GetAllAsync);
        logger.LogDebug("{serviceName}.{methodName}. Получение всех отчетов", _serviceName, methodName);

        var reports = await reportRepository.GetAllAsync(cancellationToken);
        return reports.Select(r =>
        {
            var dto = r.Adapt<ReportDto>();
            return dto with { ContentSize = r.Content.Length, DataSourceName = r.DataSource?.Name };
        }).ToList();
    }

    public async Task<ReportDto> CreateAsync(ReportModel model, CancellationToken cancellationToken = default)
    {
        var methodName = nameof(CreateAsync);
        logger.LogInformation("{serviceName}.{methodName}. Создание отчета. Name: {name}",
            _serviceName, methodName, model.Name);

        var report = model.Adapt<Domain.Entities.Report>();

        var createdReport = await reportRepository.CreateAsync(report, cancellationToken);

        logger.LogInformation("{serviceName}.{methodName}. Отчет создан. ReportId: {reportId}",
            _serviceName, methodName, createdReport.Id);

        var dto = createdReport.Adapt<ReportDto>();
        return dto with { ContentSize = createdReport.Content.Length };
    }

    public async Task<ReportDto> UpdateAsync(Guid id, ReportModel model, CancellationToken cancellationToken = default)
    {
        var methodName = nameof(UpdateAsync);
        logger.LogInformation("{serviceName}.{methodName}. Обновление отчета. ReportId: {reportId}",
            _serviceName, methodName, id);

        var report = await reportRepository.GetByIdAsync(id, cancellationToken);
        if (report == null)
        {
            logger.LogError("{serviceName}.{methodName}. Отчет не найден. ReportId: {reportId}",
                _serviceName, methodName, id);
            throw new InvalidOperationException($"Отчет с Id {id} не найден");
        }

        report.Name = model.Name;
        report.Description = model.Description;
        if (model.Content.Length > 0)
            report.Content = model.Content;
        report.DataSourceId = model.DataSourceId;
        report.IsActive = model.IsActive;

        var updatedReport = await reportRepository.UpdateAsync(report, cancellationToken);

        logger.LogInformation("{serviceName}.{methodName}. Отчет обновлен. ReportId: {reportId}",
            _serviceName, methodName, updatedReport.Id);

        var dto = updatedReport.Adapt<ReportDto>();
        return dto with { ContentSize = updatedReport.Content.Length, DataSourceName = updatedReport.DataSource?.Name };
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var methodName = nameof(DeleteAsync);
        logger.LogInformation("{serviceName}.{methodName}. Удаление отчета. ReportId: {reportId}",
            _serviceName, methodName, id);

        await reportRepository.DeleteAsync(id, cancellationToken);

        logger.LogInformation("{serviceName}.{methodName}. Отчет удален. ReportId: {reportId}",
            _serviceName, methodName, id);
    }

    public async Task<byte[]?> GenerateAsync(Guid id, string? parameters, CancellationToken cancellationToken = default)
    {
        var methodName = nameof(GenerateAsync);
        logger.LogInformation("{serviceName}.{methodName}. Генерация отчета. ReportId: {reportId}",
            _serviceName, methodName, id);

        var report = await reportRepository.GetByIdAsync(id, cancellationToken);
        if (report == null)
        {
            logger.LogError("{serviceName}.{methodName}. Отчет не найден. ReportId: {reportId}",
                _serviceName, methodName, id);
            throw new InvalidOperationException($"Отчет с Id {id} не найден");
        }

        // TODO: Реализовать генерацию отчета через Stimulsoft
        // Пока возвращаем содержимое отчета
        return report.Content;
    }
}