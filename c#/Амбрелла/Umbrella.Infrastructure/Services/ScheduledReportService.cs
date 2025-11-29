using Mapster;
using Microsoft.Extensions.Logging;
using Umbrella.Application.Contracts.Repositories;
using Umbrella.Application.Contracts.Services;
using Umbrella.Domain.Dtos;
using Umbrella.Domain.Models;

namespace Umbrella.Infrastructure.Services;

public sealed class ScheduledReportService(
    IScheduledReportRepository scheduledReportRepository,
    IReportRepository reportRepository,
    ILogger<ScheduledReportService> logger) : IScheduledReportService
{
    private readonly string _serviceName = nameof(ScheduledReportService);

    public async Task<ScheduledReportDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var methodName = nameof(GetByIdAsync);
        logger.LogDebug("{serviceName}.{methodName}. Получение запланированного отчета. ScheduledReportId: {scheduledReportId}",
            _serviceName, methodName, id);

        var scheduledReport = await scheduledReportRepository.GetByIdAsync(id, cancellationToken);
        if (scheduledReport == null)
        {
            logger.LogWarning("{serviceName}.{methodName}. Запланированный отчет не найден. ScheduledReportId: {scheduledReportId}",
                _serviceName, methodName, id);
            return null;
        }

        var dto = scheduledReport.Adapt<ScheduledReportDto>();
        return dto with { ReportName = scheduledReport.Report.Name, DataSourceName = scheduledReport.DataSource?.Name };
    }

    public async Task<List<ScheduledReportDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var methodName = nameof(GetAllAsync);
        logger.LogDebug("{serviceName}.{methodName}. Получение всех запланированных отчетов", _serviceName, methodName);

        var scheduledReports = await scheduledReportRepository.GetAllAsync(cancellationToken);
        return scheduledReports.Select(s =>
        {
            var dto = s.Adapt<ScheduledReportDto>();
            return dto with { ReportName = s.Report.Name, DataSourceName = s.DataSource?.Name };
        }).ToList();
    }

    public async Task<ScheduledReportDto> CreateAsync(ScheduledReportModel model, CancellationToken cancellationToken = default)
    {
        var methodName = nameof(CreateAsync);
        logger.LogInformation("{serviceName}.{methodName}. Создание запланированного отчета. Name: {name}",
            _serviceName, methodName, model.Name);

        if (!await reportRepository.ExistsAsync(model.ReportId, cancellationToken))
        {
            logger.LogError("{serviceName}.{methodName}. Отчет не найден. ReportId: {reportId}",
                _serviceName, methodName, model.ReportId);
            throw new InvalidOperationException($"Отчет с Id {model.ReportId} не найден");
        }

        var scheduledReport = model.Adapt<Domain.Entities.ScheduledReport>();

        var createdScheduledReport = await scheduledReportRepository.CreateAsync(scheduledReport, cancellationToken);

        logger.LogInformation("{serviceName}.{methodName}. Запланированный отчет создан. ScheduledReportId: {scheduledReportId}",
            _serviceName, methodName, createdScheduledReport.Id);

        var dto = createdScheduledReport.Adapt<ScheduledReportDto>();
        return dto with { ReportName = createdScheduledReport.Report.Name };
    }

    public async Task<ScheduledReportDto> UpdateAsync(Guid id, ScheduledReportModel model, CancellationToken cancellationToken = default)
    {
        var methodName = nameof(UpdateAsync);
        logger.LogInformation("{serviceName}.{methodName}. Обновление запланированного отчета. ScheduledReportId: {scheduledReportId}",
            _serviceName, methodName, id);

        var scheduledReport = await scheduledReportRepository.GetByIdAsync(id, cancellationToken);
        if (scheduledReport == null)
        {
            logger.LogError("{serviceName}.{methodName}. Запланированный отчет не найден. ScheduledReportId: {scheduledReportId}",
                _serviceName, methodName, id);
            throw new InvalidOperationException($"Запланированный отчет с Id {id} не найден");
        }

        scheduledReport.ReportId = model.ReportId;
        scheduledReport.DataSourceId = model.DataSourceId;
        scheduledReport.Schedule = model.Schedule;
        scheduledReport.Name = model.Name;
        scheduledReport.Description = model.Description;
        scheduledReport.Parameters = model.Parameters;
        scheduledReport.IsActive = model.IsActive;

        var updatedScheduledReport = await scheduledReportRepository.UpdateAsync(scheduledReport, cancellationToken);

        logger.LogInformation("{serviceName}.{methodName}. Запланированный отчет обновлен. ScheduledReportId: {scheduledReportId}",
            _serviceName, methodName, updatedScheduledReport.Id);

        var dto = updatedScheduledReport.Adapt<ScheduledReportDto>();
        return dto with { ReportName = updatedScheduledReport.Report.Name, DataSourceName = updatedScheduledReport.DataSource?.Name };
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var methodName = nameof(DeleteAsync);
        logger.LogInformation("{serviceName}.{methodName}. Удаление запланированного отчета. ScheduledReportId: {scheduledReportId}",
            _serviceName, methodName, id);

        await scheduledReportRepository.DeleteAsync(id, cancellationToken);

        logger.LogInformation("{serviceName}.{methodName}. Запланированный отчет удален. ScheduledReportId: {scheduledReportId}",
            _serviceName, methodName, id);
    }
}