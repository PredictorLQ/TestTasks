using MediatR;
using OrderTrackingApplication.Domain.Dtos;
using OrderTrackingApplication.Domain.Enums;

namespace OrderTrackingApplication.Application.Commands.Orders.UpdateOrderStatus;

public sealed partial class UpdateOrderStatusCommand(
    Guid orderId,
    OrderStatus status)
    : IRequest<OrderDto>
{
    public Guid OrderId { get; } = orderId;
    public OrderStatus Status { get; } = status;
}
