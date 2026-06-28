using MediatR;
using OrderTrackingApplication.Domain.Dtos;

namespace OrderTrackingApplication.Application.Queries.Orders.GetAllOrders;

public sealed partial class GetAllOrdersQuery : IRequest<IReadOnlyList<OrderDto>>;
