using MediatR;
using OrderTrackingApplication.Domain.Dtos;

namespace OrderTrackingApplication.Application.Commands.Orders.CreateOrder;

public sealed partial class CreateOrderCommand(
    string orderNumber,
    string description)
    : IRequest<OrderDto>
{
    public string OrderNumber { get; } = orderNumber;
    public string Description { get; } = description;
}
