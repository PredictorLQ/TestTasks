using MediatR;

namespace Umbrella.Application.Commands.Reports.DeleteReport;

/// <summary>
/// Команда для удаления отчета
/// </summary>
public sealed partial class DeleteReportCommand(Guid id) : IRequest
{
    public Guid Id { get; } = id;
}