using Mapster;
using MediatR;
using OrderTrackingApplication.Application.Contracts.Repositories;
using OrderTrackingApplication.Domain.Dtos;

namespace OrderTrackingApplication.Application.Queries.Orders.GetAllOrders;

public sealed partial class GetAllOrdersQuery
{
    public sealed class GetAllOrdersQueryHandle(
        IOrderRepository orderRepository) : IRequestHandler<GetAllOrdersQuery, IReadOnlyList<OrderDto>>
    {
        public async Task<IReadOnlyList<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await orderRepository.GetAllAsync(cancellationToken);
            return orders.Adapt<List<OrderDto>>();
        }
    }
}
