using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using OrderTrackingApplication.Application.Contracts.Repositories;
using OrderTrackingApplication.Application.Contracts.Services;
using OrderTrackingApplication.Application.Exceptions;
using OrderTrackingApplication.Domain.Dtos;
using OrderTrackingApplication.Domain.Entities;
using OrderTrackingApplication.Domain.Events;
using System.Text.Json;

namespace OrderTrackingApplication.Application.Commands.Orders.UpdateOrderStatus;

public sealed partial class UpdateOrderStatusCommand
{
    public sealed class UpdateOrderStatusCommandHandle(
        IOrderRepository orderRepository,
        IOutboxRepository outboxRepository,
        IOrderNotificationService notificationService,
        ILogger<UpdateOrderStatusCommandHandle> logger) : IRequestHandler<UpdateOrderStatusCommand, OrderDto>
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public async Task<OrderDto> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetByIdForUpdateAsync(request.OrderId, cancellationToken)
                ?? throw new NotFoundException($"Заказ с Id {request.OrderId} не найден.");

            var previousStatus = order.Status;

            if (previousStatus == request.Status)
            {
                return order.Adapt<OrderDto>();
            }

            order.UpdateStatus(request.Status);

            var domainEvent = new OrderStatusChangedEvent(
                EventId: Guid.NewGuid(),
                OrderId: order.Id,
                OrderNumber: order.OrderNumber,
                PreviousStatus: previousStatus,
                NewStatus: request.Status,
                OccurredAt: DateTime.UtcNow);

            var outboxMessage = new OutboxMessage
            {
                EventType = nameof(OrderStatusChangedEvent),
                Payload = JsonSerializer.Serialize(domainEvent, JsonOptions),
                IdempotencyKey = domainEvent.EventId.ToString()
            };

            await orderRepository.UpdateAsync(order, cancellationToken);
            await outboxRepository.AddAsync(outboxMessage, cancellationToken);
            await orderRepository.SaveChangesAsync(cancellationToken);

            await notificationService.NotifyStatusChangedAsync(
                order.Id,
                order.OrderNumber,
                previousStatus,
                request.Status,
                cancellationToken);

            logger.LogInformation(
                "Статус заказа изменён. OrderId: {OrderId}, {PreviousStatus} -> {NewStatus}",
                order.Id,
                previousStatus,
                request.Status);

            return order.Adapt<OrderDto>();
        }
    }
}
