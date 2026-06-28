using OrderTrackingApplication.Application.Contracts.Services;
using OrderTrackingApplication.Domain.Enums;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading.Channels;

namespace OrderTrackingApplication.Infrastructure.Services;

public sealed class OrderNotificationService : IOrderNotificationService
{
    private readonly ConcurrentDictionary<Guid, Channel<OrderStatusNotification>> _subscribers = new();

    public Task NotifyStatusChangedAsync(
        Guid orderId,
        string orderNumber,
        OrderStatus previousStatus,
        OrderStatus newStatus,
        CancellationToken cancellationToken = default)
    {
        var notification = new OrderStatusNotification(
            orderId,
            orderNumber,
            previousStatus,
            newStatus,
            DateTime.UtcNow);

        foreach (var (subscriberId, channel) in _subscribers)
        {
            channel.Writer.TryWrite(notification);
        }

        return Task.CompletedTask;
    }

    public async IAsyncEnumerable<OrderStatusNotification> SubscribeAsync(
        Guid? orderId = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var channel = Channel.CreateUnbounded<OrderStatusNotification>(
            new UnboundedChannelOptions { SingleReader = true });

        var subscriberId = Guid.NewGuid();
        _subscribers[subscriberId] = channel;

        try
        {
            await foreach (var notification in channel.Reader.ReadAllAsync(cancellationToken))
            {
                if (orderId is null || notification.OrderId == orderId)
                {
                    yield return notification;
                }
            }
        }
        finally
        {
            _subscribers.TryRemove(subscriberId, out _);
            channel.Writer.TryComplete();
        }
    }
}
