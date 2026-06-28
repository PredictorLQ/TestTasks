using OrderTrackingApplication.Domain.Enums;

namespace OrderTrackingApplication.Domain.Events;

public sealed record OrderStatusChangedEvent(
    Guid EventId,
    Guid OrderId,
    string OrderNumber,
    OrderStatus PreviousStatus,
    OrderStatus NewStatus,
    DateTime OccurredAt);
