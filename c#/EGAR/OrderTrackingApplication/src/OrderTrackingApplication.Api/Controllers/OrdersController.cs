using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderTrackingApplication.Api.Bindings;
using OrderTrackingApplication.Application.Commands.Orders.CreateOrder;
using OrderTrackingApplication.Application.Commands.Orders.UpdateOrderStatus;
using OrderTrackingApplication.Application.Exceptions;
using OrderTrackingApplication.Application.Queries.Orders.GetAllOrders;
using OrderTrackingApplication.Application.Queries.Orders.GetOrderById;
using OrderTrackingApplication.Application.Queries.Orders.GetOrderByNumber;
using OrderTrackingApplication.Domain.Dtos;

namespace OrderTrackingApplication.Api.Controllers;

/// <summary>
/// Контроллер для управления заказами
/// </summary>
[ApiController]
[Route("api/[controller]")]
public sealed class OrdersController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Создать новый заказ
    /// </summary>
    /// <param name="binding">Данные для создания заказа</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Созданный заказ</returns>
    [HttpPost]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateOrderBinding binding,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await mediator.Send(
                new CreateOrderCommand(binding.OrderNumber, binding.Description),
                cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Errors);
        }
    }

    /// <summary>
    /// Получить список всех заказов
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список заказов</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<OrderDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllOrdersQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Получить заказ по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заказа</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Заказ</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await mediator.Send(new GetOrderByIdQuery(id), cancellationToken);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Получить заказ по номеру
    /// </summary>
    /// <param name="orderNumber">Номер заказа</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Заказ</returns>
    [HttpGet("by-number/{orderNumber}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByNumber(string orderNumber, CancellationToken cancellationToken)
    {
        try
        {
            var result = await mediator.Send(new GetOrderByNumberQuery(orderNumber), cancellationToken);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Изменить статус заказа
    /// </summary>
    /// <param name="id">Идентификатор заказа</param>
    /// <param name="binding">Новый статус заказа</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Обновлённый заказ</returns>
    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateStatus(
        Guid id,
        [FromBody] UpdateOrderStatusBinding binding,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await mediator.Send(
                new UpdateOrderStatusCommand(id, binding.Status),
                cancellationToken);

            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Errors);
        }
    }
}
