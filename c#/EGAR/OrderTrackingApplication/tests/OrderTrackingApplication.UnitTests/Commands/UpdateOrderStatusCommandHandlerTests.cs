using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using OrderTrackingApplication.Application.Commands.Orders.UpdateOrderStatus;
using OrderTrackingApplication.Application.Contracts.Repositories;
using OrderTrackingApplication.Application.Contracts.Services;
using OrderTrackingApplication.Application.Exceptions;
using OrderTrackingApplication.Domain.Entities;
using OrderTrackingApplication.Domain.Enums;

namespace OrderTrackingApplication.UnitTests.Commands;

public class UpdateOrderStatusCommandHandlerTests : UnitTestBase
{
    private readonly Mock<IOrderRepository> _orderRepository = new();
    private readonly Mock<IOutboxRepository> _outboxRepository = new();
    private readonly Mock<IOrderNotificationService> _notificationService = new();
    private readonly UpdateOrderStatusCommand.UpdateOrderStatusCommandHandle _handler;

    public UpdateOrderStatusCommandHandlerTests()
    {
        _handler = new UpdateOrderStatusCommand.UpdateOrderStatusCommandHandle(
            _orderRepository.Object,
            _outboxRepository.Object,
            _notificationService.Object,
            Mock.Of<ILogger<UpdateOrderStatusCommand.UpdateOrderStatusCommandHandle>>());
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenOrderDoesNotExist()
    {
        var orderId = Guid.NewGuid();
        var command = new UpdateOrderStatusCommand(orderId, OrderStatus.Sent);

        _orderRepository
            .Setup(x => x.GetByIdForUpdateAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Order?)null);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_UpdatesStatus_AddsOutboxMessage_AndNotifies()
    {
        var orderId = Guid.NewGuid();
        var order = new Order
        {
            Id = orderId,
            OrderNumber = "ORD-001",
            Description = "Test",
            Status = OrderStatus.Created
        };
        var command = new UpdateOrderStatusCommand(orderId, OrderStatus.Sent);

        _orderRepository
            .Setup(x => x.GetByIdForUpdateAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        OutboxMessage? capturedOutbox = null;
        _outboxRepository
            .Setup(x => x.AddAsync(It.IsAny<OutboxMessage>(), It.IsAny<CancellationToken>()))
            .Callback<OutboxMessage, CancellationToken>((message, _) => capturedOutbox = message)
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Status.Should().Be(OrderStatus.Sent);
        capturedOutbox.Should().NotBeNull();
        capturedOutbox!.EventType.Should().Be("OrderStatusChangedEvent");
        capturedOutbox.IdempotencyKey.Should().NotBeNullOrEmpty();

        _orderRepository.Verify(x => x.UpdateAsync(order, It.IsAny<CancellationToken>()), Times.Once);
        _orderRepository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _notificationService.Verify(
            x => x.NotifyStatusChangedAsync(
                orderId,
                "ORD-001",
                OrderStatus.Created,
                OrderStatus.Sent,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ReturnsOrderWithoutSideEffects_WhenStatusUnchanged()
    {
        var orderId = Guid.NewGuid();
        var order = new Order
        {
            Id = orderId,
            OrderNumber = "ORD-001",
            Description = "Test",
            Status = OrderStatus.Sent
        };
        var command = new UpdateOrderStatusCommand(orderId, OrderStatus.Sent);

        _orderRepository
            .Setup(x => x.GetByIdForUpdateAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Status.Should().Be(OrderStatus.Sent);
        _outboxRepository.Verify(x => x.AddAsync(It.IsAny<OutboxMessage>(), It.IsAny<CancellationToken>()), Times.Never);
        _notificationService.Verify(
            x => x.NotifyStatusChangedAsync(
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<OrderStatus>(),
                It.IsAny<OrderStatus>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
