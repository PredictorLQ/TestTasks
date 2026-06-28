using FluentAssertions;
using OrderTrackingApplication.Application.Contracts.Services;
using OrderTrackingApplication.Domain.Enums;
using OrderTrackingApplication.Infrastructure.Services;

namespace OrderTrackingApplication.UnitTests.Services;

public class OrderNotificationServiceTests
{
    [Fact]
    public async Task NotifyStatusChangedAsync_DeliversNotificationToSubscriber()
    {
        var service = new OrderNotificationService();
        var orderId = Guid.NewGuid();
        using var cts = new CancellationTokenSource();

        var subscriptionTask = Task.Run(async () =>
        {
            await foreach (var notification in service.SubscribeAsync(orderId, cts.Token))
            {
                notification.OrderId.Should().Be(orderId);
                notification.PreviousStatus.Should().Be(OrderStatus.Created);
                notification.NewStatus.Should().Be(OrderStatus.Sent);
                return;
            }
        });

        await Task.Delay(100);
        await service.NotifyStatusChangedAsync(
            orderId,
            "ORD-001",
            OrderStatus.Created,
            OrderStatus.Sent);

        await subscriptionTask.WaitAsync(TimeSpan.FromSeconds(2));
        cts.Cancel();
    }

    [Fact]
    public async Task SubscribeAsync_FiltersByOrderId_WhenFilterSpecified()
    {
        var service = new OrderNotificationService();
        var targetOrderId = Guid.NewGuid();
        var otherOrderId = Guid.NewGuid();
        using var cts = new CancellationTokenSource();

        var received = new List<OrderStatusNotification>();
        var subscriptionTask = Task.Run(async () =>
        {
            await foreach (var notification in service.SubscribeAsync(targetOrderId, cts.Token))
            {
                received.Add(notification);
                if (received.Count == 1)
                {
                    break;
                }
            }
        });

        await Task.Delay(100);
        await service.NotifyStatusChangedAsync(
            otherOrderId,
            "ORD-OTHER",
            OrderStatus.Created,
            OrderStatus.Sent);
        await service.NotifyStatusChangedAsync(
            targetOrderId,
            "ORD-TARGET",
            OrderStatus.Created,
            OrderStatus.Sent);

        await subscriptionTask.WaitAsync(TimeSpan.FromSeconds(2));
        cts.Cancel();

        received.Should().ContainSingle();
        received[0].OrderId.Should().Be(targetOrderId);
    }
}
