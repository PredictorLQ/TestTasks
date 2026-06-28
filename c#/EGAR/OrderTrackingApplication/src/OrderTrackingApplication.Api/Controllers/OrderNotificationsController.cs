using Microsoft.AspNetCore.Mvc;
using OrderTrackingApplication.Application.Contracts.Services;
using System.Text.Json;

namespace OrderTrackingApplication.Api.Controllers;

/// <summary>
/// Контроллер для real-time уведомлений об изменении статуса заказов
/// </summary>
[ApiController]
[Route("api/[controller]")]
public sealed class OrderNotificationsController(
    IOrderNotificationService notificationService) : ControllerBase
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    /// <summary>
    /// Подписка на уведомления через Server-Sent Events (SSE)
    /// </summary>
    /// <param name="orderId">Фильтр по идентификатору заказа (необязательно)</param>
    /// <param name="cancellationToken">Токен отмены</param>
    [HttpGet("stream")]
    [Produces("text/event-stream")]
    public async Task Stream(
        [FromQuery] Guid? orderId,
        CancellationToken cancellationToken)
    {
        Response.Headers.Append("Content-Type", "text/event-stream");
        Response.Headers.Append("Cache-Control", "no-cache");
        Response.Headers.Append("Connection", "keep-alive");

        await foreach (var notification in notificationService.SubscribeAsync(orderId, cancellationToken))
        {
            var payload = JsonSerializer.Serialize(notification, JsonOptions);
            await Response.WriteAsync($"data: {payload}\n\n", cancellationToken);
            await Response.Body.FlushAsync(cancellationToken);
        }
    }
}
