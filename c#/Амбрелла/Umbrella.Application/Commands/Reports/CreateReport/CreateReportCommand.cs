using MediatR;
using Umbrella.Domain.Dtos;

namespace Umbrella.Application.Commands.Reports.CreateReport;

/// <summary>
/// Команда для создания нового отчета
/// </summary>
public sealed partial class CreateReportCommand(
    string name,
    string? description,
    byte[] content,
    Guid? dataSourceId,
    bool isActive)
    : IRequest<ReportDto>
{
    public string Name { get; } = name;
    public string? Description { get; } = description;
    public byte[] Content { get; } = content;
    public Guid? DataSourceId { get; } = dataSourceId;
    public bool IsActive { get; } = isActive;
}