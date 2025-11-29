using MediatR;
using Umbrella.Domain.Dtos;

namespace Umbrella.Application.Commands.ScheduledReports.CreateScheduledReport;

/// <summary>
/// Команда для создания нового запланированного отчета
/// </summary>
public sealed partial class CreateScheduledReportCommand(
    Guid reportId,
    Guid? dataSourceId,
    string schedule,
    string name,
    string? description,
    string? parameters,
    bool isActive)
    : IRequest<ScheduledReportDto>
{
    public Guid ReportId { get; } = reportId;
    public Guid? DataSourceId { get; } = dataSourceId;
    public string Schedule { get; } = schedule;
    public string Name { get; } = name;
    public string? Description { get; } = description;
    public string? Parameters { get; } = parameters;
    public bool IsActive { get; } = isActive;
}