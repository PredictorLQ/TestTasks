using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Umbrella.Api.Bindings;
using Umbrella.Application.Commands.ScheduledReports.CreateScheduledReport;
using Umbrella.Application.Queries.ScheduledReports.GetAllScheduledReports;
using Umbrella.Domain.Dtos;

namespace Umbrella.Api.Controllers;

/// <summary>
/// Контроллер для управления запланированными отчетами
/// </summary>
[ApiController]
[Route("api/[controller]")]
public sealed class ScheduledReportsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Получить все запланированные отчеты
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<ScheduledReportDto>), StatusCodes.Status200OK)]
    public Task<List<ScheduledReportDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return mediator.Send(new GetAllScheduledReportsQuery(), cancellationToken);
    }

    /// <summary>
    /// Создать новый запланированный отчет
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ScheduledReportDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public Task<ScheduledReportDto> CreateAsync([FromBody] CreateScheduledReportBinding binding, CancellationToken cancellationToken = default)
    {
        var command = binding.Adapt<CreateScheduledReportCommand>();
        return mediator.Send(command, cancellationToken);
    }
}