using MediatR;
using Microsoft.Extensions.Logging;
using Umbrella.Application.Contracts.Services;
using Umbrella.Domain.Dtos;

namespace Umbrella.Application.Queries.Reports.GetAllReports;

public sealed partial class GetAllReportsQuery
{
    public sealed class GetAllReportsQueryHandle(
        IReportService reportService,
        ILogger<GetAllReportsQueryHandle> logger) : IRequestHandler<GetAllReportsQuery, List<ReportDto>>
    {
        public async Task<List<ReportDto>> Handle(GetAllReportsQuery request, CancellationToken cancellationToken)
        {
            var methodName = nameof(Handle);
            logger.LogDebug("GetAllReportsQuery.{methodName}. Получение всех отчетов", methodName);

            var result = await reportService.GetAllAsync(cancellationToken);

            logger.LogDebug("GetAllReportsQuery.{methodName}. Получено отчетов: {count}",
                methodName, result.Count);

            return result;
        }
    }
}