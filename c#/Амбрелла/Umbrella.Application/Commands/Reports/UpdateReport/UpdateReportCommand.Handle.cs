using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Umbrella.Application.Contracts.Services;
using Umbrella.Domain.Dtos;
using Umbrella.Domain.Models;

namespace Umbrella.Application.Commands.Reports.UpdateReport;

public sealed partial class UpdateReportCommand
{
    public sealed class UpdateReportCommandHandle(
        IReportService reportService,
        ILogger<UpdateReportCommandHandle> logger) : IRequestHandler<UpdateReportCommand, ReportDto>
    {
        public async Task<ReportDto> Handle(UpdateReportCommand request, CancellationToken cancellationToken)
        {
            var methodName = nameof(Handle);
            logger.LogInformation("UpdateReportCommand.{methodName}. Начало обновления отчета. ReportId: {reportId}",
                methodName, request.Id);

            var model = request.Adapt<ReportModel>();

            var result = await reportService.UpdateAsync(request.Id, model, cancellationToken);

            logger.LogInformation("UpdateReportCommand.{methodName}. Отчет обновлен. ReportId: {reportId}",
                methodName, request.Id);

            return result;
        }
    }
}