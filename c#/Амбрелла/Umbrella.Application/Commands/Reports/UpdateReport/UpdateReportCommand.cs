using MediatR;
using Umbrella.Domain.Dtos;

namespace Umbrella.Application.Commands.Reports.UpdateReport;

/// <summary>
/// Команда для обновления отчета
/// </summary>
public sealed partial class UpdateReportCommand(
    Guid id,
    string name,
    string? description,
    byte[]? content,
    Guid? dataSourceId,
    bool isActive)
    : IRequest<ReportDto>
{
    public Guid Id { get; } = id;
    public string Name { get; } = name;
    public string? Description { get; } = description;
    public byte[]? Content { get; } = content;
    public Guid? DataSourceId { get; } = dataSourceId;
    public bool IsActive { get; } = isActive;
}