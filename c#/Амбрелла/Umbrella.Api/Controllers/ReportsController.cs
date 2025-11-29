using MediatR;
using Microsoft.AspNetCore.Mvc;
using Umbrella.Api.Bindings;
using Umbrella.Application.Commands.Reports.CreateReport;
using Umbrella.Application.Commands.Reports.DeleteReport;
using Umbrella.Application.Queries.Reports.GetAllReports;
using Umbrella.Application.Queries.Reports.GetReportById;
using Umbrella.Domain.Dtos;

namespace Umbrella.Api.Controllers;

/// <summary>
/// Контроллер для управления отчетами
/// </summary>
[ApiController]
[Route("api/[controller]")]
public sealed class ReportsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Получить все отчеты
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<ReportDto>), StatusCodes.Status200OK)]
    public Task<List<ReportDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return mediator.Send(new GetAllReportsQuery(), cancellationToken);
    }

    /// <summary>
    /// Получить отчет по идентификатору
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ReportDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<ReportDto?> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        return mediator.Send(new GetReportByIdQuery(id), cancellationToken);
    }

    /// <summary>
    /// Создать новый отчет
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ReportDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public Task<ReportDto> CreateAsync([FromBody] CreateReportBinding binding, CancellationToken cancellationToken = default)
    {
        var content = Convert.FromBase64String(binding.ContentBase64);
        var command = new CreateReportCommand(
            binding.Name,
            binding.Description,
            content,
            binding.DataSourceId,
            binding.IsActive);

        return mediator.Send(command, cancellationToken);
    }

    /// <summary>
    /// Удалить отчет
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        await mediator.Send(new DeleteReportCommand(id), cancellationToken);
        return NoContent();
    }
}