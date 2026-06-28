using OrderTrackingApplication.Application.Contracts.Services;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace OrderTrackingApplication.Api.WebSockets;

public sealed class OrderStatusWebSocketHandler(
    RequestDelegate next,
    ILogger<OrderStatusWebSocketHandler> logger)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task InvokeAsync(HttpContext context, IOrderNotificationService notificationService)
    {
        if (!context.Request.Path.Equals("/ws/orders", StringComparison.OrdinalIgnoreCase))
        {
            await next(context);
            return;
        }

        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }

        using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        Guid? orderId = null;

        if (context.Request.Query.TryGetValue("orderId", out var orderIdValue) &&
            Guid.TryParse(orderIdValue, out var parsedOrderId))
        {
            orderId = parsedOrderId;
        }

        logger.LogInformation("WebSocket подключение установлено. OrderId filter: {OrderId}", orderId);

        try
        {
            await foreach (var notification in notificationService.SubscribeAsync(orderId, context.RequestAborted))
            {
                var payload = JsonSerializer.Serialize(notification, JsonOptions);
                var bytes = Encoding.UTF8.GetBytes(payload);

                await webSocket.SendAsync(
                    bytes,
                    WebSocketMessageType.Text,
                    endOfMessage: true,
                    context.RequestAborted);
            }
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("WebSocket соединение закрыто.");
        }
        catch (WebSocketException ex)
        {
            logger.LogWarning(ex, "Ошибка WebSocket соединения.");
        }
    }
}

public static class OrderStatusWebSocketExtensions
{
    public static IApplicationBuilder UseOrderStatusWebSockets(this IApplicationBuilder app)
    {
        app.UseWebSockets();
        app.UseMiddleware<OrderStatusWebSocketHandler>();
        return app;
    }
}
