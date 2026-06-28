using Mapster;
using MediatR;
using OrderTrackingApplication.Application.Contracts.Repositories;
using OrderTrackingApplication.Application.Exceptions;
using OrderTrackingApplication.Domain.Dtos;

namespace OrderTrackingApplication.Application.Queries.Orders.GetOrderByNumber;

public sealed partial class GetOrderByNumberQuery
{
    public sealed class GetOrderByNumberQueryHandle(
        IOrderRepository orderRepository) : IRequestHandler<GetOrderByNumberQuery, OrderDto>
    {
        public async Task<OrderDto> Handle(GetOrderByNumberQuery request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetByOrderNumberAsync(request.OrderNumber, cancellationToken)
                ?? throw new NotFoundException($"Заказ с номером {request.OrderNumber} не найден.");

            return order.Adapt<OrderDto>();
        }
    }
}
