using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using OrderTrackingApplication.Application.Contracts.Repositories;
using OrderTrackingApplication.Domain.Dtos;
using OrderTrackingApplication.Domain.Entities;

namespace OrderTrackingApplication.Application.Commands.Orders.CreateOrder;

public sealed partial class CreateOrderCommand
{
    public sealed class CreateOrderCommandHandle(
        IOrderRepository orderRepository,
        ILogger<CreateOrderCommandHandle> logger) : IRequestHandler<CreateOrderCommand, OrderDto>
    {
        public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "Создание заказа. OrderNumber: {OrderNumber}",
                request.OrderNumber);

            if (await orderRepository.ExistsByOrderNumberAsync(request.OrderNumber, cancellationToken))
            {
                throw new ValidationException([
                    new FluentValidation.Results.ValidationFailure(
                        nameof(request.OrderNumber),
                        $"Заказ с номером {request.OrderNumber} уже существует.")
                ]);
            }

            var order = new Order
            {
                OrderNumber = request.OrderNumber,
                Description = request.Description
            };

            await orderRepository.AddAsync(order, cancellationToken);
            await orderRepository.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Заказ создан. OrderId: {OrderId}, OrderNumber: {OrderNumber}",
                order.Id,
                order.OrderNumber);

            return order.Adapt<OrderDto>();
        }
    }
}
