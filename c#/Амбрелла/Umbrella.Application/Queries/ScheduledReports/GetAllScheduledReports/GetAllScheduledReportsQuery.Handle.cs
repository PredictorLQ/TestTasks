using MediatR;
using Microsoft.Extensions.Logging;
using Umbrella.Application.Contracts.Services;
using Umbrella.Domain.Dtos;

namespace Umbrella.Application.Queries.ScheduledReports.GetAllScheduledReports;

public sealed partial class GetAllScheduledReportsQuery
{
    public sealed class GetAllScheduledReportsQueryHandle(
        IScheduledReportService scheduledReportService,
        ILogger<GetAllScheduledReportsQueryHandle> logger) : IRequestHandler<GetAllScheduledReportsQuery, List<ScheduledReportDto>>
    {
        public async Task<List<ScheduledReportDto>> Handle(GetAllScheduledReportsQuery request, CancellationToken cancellationToken)
        {
            var methodName = nameof(Handle);
            logger.LogDebug("GetAllScheduledReportsQuery.{methodName}. Получение всех запланированных отчетов", methodName);

            var result = await scheduledReportService.GetAllAsync(cancellationToken);

            logger.LogDebug("GetAllScheduledReportsQuery.{methodName}. Получено запланированных отчетов: {count}",
                methodName, result.Count);

            return result;
        }
    }
}