using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Umbrella.Api.Bindings;
using Umbrella.Application.Commands.DataSources.CreateDataSource;
using Umbrella.Application.Commands.DataSources.DeleteDataSource;
using Umbrella.Application.Commands.DataSources.UpdateDataSource;
using Umbrella.Application.Queries.DataSources.GetAllDataSources;
using Umbrella.Application.Queries.DataSources.GetDataSourceById;
using Umbrella.Domain.Dtos;

namespace Umbrella.Api.Controllers;

/// <summary>
/// Контроллер для управления источниками данных
/// </summary>
[ApiController]
[Route("api/[controller]")]
public sealed class DataSourcesController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Получить все источники данных
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<DataSourceDto>), StatusCodes.Status200OK)]
    public Task<List<DataSourceDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return mediator.Send(new GetAllDataSourcesQuery(), cancellationToken);
    }

    /// <summary>
    /// Получить источник данных по идентификатору
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(DataSourceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<DataSourceDto?> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        return mediator.Send(new GetDataSourceByIdQuery(id), cancellationToken);
    }

    /// <summary>
    /// Создать новый источник данных
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(DataSourceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public Task<DataSourceDto> CreateAsync([FromBody] CreateDataSourceBinding binding, CancellationToken cancellationToken = default)
    {
        var command = binding.Adapt<CreateDataSourceCommand>();
        return mediator.Send(command, cancellationToken);
    }

    /// <summary>
    /// Обновить источник данных
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(DataSourceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public Task<DataSourceDto> UpdateAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateDataSourceBinding binding,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateDataSourceCommand(
            id,
            binding.Name,
            binding.Type,
            binding.ConnectionString,
            binding.Description,
            binding.IsActive);
        return mediator.Send(command, cancellationToken);
    }

    /// <summary>
    /// Удалить источник данных
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        await mediator.Send(new DeleteDataSourceCommand(id), cancellationToken);
        return NoContent();
    }
}