using MediatR;
using Microsoft.Extensions.Logging;
using Umbrella.Application.Contracts.Services;

namespace Umbrella.Application.Commands.Reports.DeleteReport;

public sealed partial class DeleteReportCommand
{
    public sealed class DeleteReportCommandHandle(
        IReportService reportService,
        ILogger<DeleteReportCommandHandle> logger) : IRequestHandler<DeleteReportCommand>
    {
        public async Task Handle(DeleteReportCommand request, CancellationToken cancellationToken)
        {
            var methodName = nameof(Handle);
            logger.LogInformation("DeleteReportCommand.{methodName}. Начало удаления отчета. ReportId: {reportId}",
                methodName, request.Id);

            await reportService.DeleteAsync(request.Id, cancellationToken);

            logger.LogInformation("DeleteReportCommand.{methodName}. Отчет удален. ReportId: {reportId}",
                methodName, request.Id);
        }
    }
}