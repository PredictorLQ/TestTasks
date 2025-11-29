using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Umbrella.Application.Contracts.Services;
using Umbrella.Domain.Dtos;
using Umbrella.Domain.Models;

namespace Umbrella.Application.Commands.Reports.CreateReport;

public sealed partial class CreateReportCommand
{
    public sealed class CreateReportCommandHandle(
        IReportService reportService,
        ILogger<CreateReportCommandHandle> logger) : IRequestHandler<CreateReportCommand, ReportDto>
    {
        public async Task<ReportDto> Handle(CreateReportCommand request, CancellationToken cancellationToken)
        {
            var methodName = nameof(Handle);
            logger.LogInformation("CreateReportCommand.{methodName}. Начало создания отчета. Name: {name}",
                methodName, request.Name);

            var model = request.Adapt<ReportModel>();

            var result = await reportService.CreateAsync(model, cancellationToken);

            logger.LogInformation("CreateReportCommand.{methodName}. Отчет создан. ReportId: {reportId}",
                methodName, result.Id);

            return result;
        }
    }
}