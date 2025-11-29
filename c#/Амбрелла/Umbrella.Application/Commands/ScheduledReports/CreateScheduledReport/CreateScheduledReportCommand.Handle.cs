using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Umbrella.Application.Contracts.Services;
using Umbrella.Domain.Dtos;
using Umbrella.Domain.Models;

namespace Umbrella.Application.Commands.ScheduledReports.CreateScheduledReport;

public sealed partial class CreateScheduledReportCommand
{
    public sealed class CreateScheduledReportCommandHandle(
        IScheduledReportService scheduledReportService,
        ILogger<CreateScheduledReportCommandHandle> logger) : IRequestHandler<CreateScheduledReportCommand, ScheduledReportDto>
    {
        public async Task<ScheduledReportDto> Handle(CreateScheduledReportCommand request, CancellationToken cancellationToken)
        {
            var methodName = nameof(Handle);
            logger.LogInformation("CreateScheduledReportCommand.{methodName}. Начало создания запланированного отчета. Name: {name}",
                methodName, request.Name);

            var model = request.Adapt<ScheduledReportModel>();

            var result = await scheduledReportService.CreateAsync(model, cancellationToken);

            logger.LogInformation("CreateScheduledReportCommand.{methodName}. Запланированный отчет создан. ScheduledReportId: {scheduledReportId}",
                methodName, result.Id);

            return result;
        }
    }
}