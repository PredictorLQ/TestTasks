using MediatR;
using OrderTrackingApplication.Domain.Dtos;

namespace OrderTrackingApplication.Application.Queries.Orders.GetOrderByNumber;

public sealed partial class GetOrderByNumberQuery(string orderNumber) : IRequest<OrderDto>
{
    public string OrderNumber { get; } = orderNumber;
}
