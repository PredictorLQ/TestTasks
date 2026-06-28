using MediatR;
using OrderTrackingApplication.Domain.Dtos;

namespace OrderTrackingApplication.Application.Queries.Orders.GetOrderById;

public sealed partial class GetOrderByIdQuery(Guid orderId) : IRequest<OrderDto>
{
    public Guid OrderId { get; } = orderId;
}
