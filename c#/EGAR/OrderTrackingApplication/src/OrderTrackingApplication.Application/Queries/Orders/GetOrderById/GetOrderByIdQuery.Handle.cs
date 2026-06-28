using Mapster;
using MediatR;
using OrderTrackingApplication.Application.Contracts.Repositories;
using OrderTrackingApplication.Application.Exceptions;
using OrderTrackingApplication.Domain.Dtos;

namespace OrderTrackingApplication.Application.Queries.Orders.GetOrderById;

public sealed partial class GetOrderByIdQuery
{
    public sealed class GetOrderByIdQueryHandle(
        IOrderRepository orderRepository) : IRequestHandler<GetOrderByIdQuery, OrderDto>
    {
        public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetByIdAsync(request.OrderId, cancellationToken)
                ?? throw new NotFoundException($"Заказ с Id {request.OrderId} не найден.");

            return order.Adapt<OrderDto>();
        }
    }
}
