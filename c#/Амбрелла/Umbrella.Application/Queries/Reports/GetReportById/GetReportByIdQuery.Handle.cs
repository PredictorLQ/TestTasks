using MediatR;
using Microsoft.Extensions.Logging;
using Umbrella.Application.Contracts.Services;
using Umbrella.Domain.Dtos;

namespace Umbrella.Application.Queries.Reports.GetReportById;

public sealed partial class GetReportByIdQuery
{
    public sealed class GetReportByIdQueryHandle(
        IReportService reportService,
        ILogger<GetReportByIdQueryHandle> logger) : IRequestHandler<GetReportByIdQuery, ReportDto?>
    {
        public async Task<ReportDto?> Handle(GetReportByIdQuery request, CancellationToken cancellationToken)
        {
            var methodName = nameof(Handle);
            logger.LogDebug("GetReportByIdQuery.{methodName}. Получение отчета. ReportId: {reportId}",
                methodName, request.Id);

            var result = await reportService.GetByIdAsync(request.Id, cancellationToken);

            logger.LogDebug("GetReportByIdQuery.{methodName}. Отчет {found}. ReportId: {reportId}",
                methodName, result != null ? "найден" : "не найден", request.Id);

            return result;
        }
    }
}