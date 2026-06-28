using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using OrderTrackingApplication.Application.Commands.Orders.CreateOrder;
using OrderTrackingApplication.Application.Contracts.Repositories;
using OrderTrackingApplication.Domain.Entities;

namespace OrderTrackingApplication.UnitTests.Commands;

public class CreateOrderCommandHandlerTests : UnitTestBase
{
    private readonly Mock<IOrderRepository> _orderRepository = new();
    private readonly CreateOrderCommand.CreateOrderCommandHandle _handler;

    public CreateOrderCommandHandlerTests()
    {
        _handler = new CreateOrderCommand.CreateOrderCommandHandle(
            _orderRepository.Object,
            Mock.Of<ILogger<CreateOrderCommand.CreateOrderCommandHandle>>());
    }

    [Fact]
    public async Task Handle_CreatesOrder_WhenOrderNumberIsUnique()
    {
        var command = new CreateOrderCommand("ORD-001", "Test order");

        _orderRepository
            .Setup(x => x.ExistsByOrderNumberAsync(command.OrderNumber, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        Order? capturedOrder = null;
        _orderRepository
            .Setup(x => x.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .Callback<Order, CancellationToken>((order, _) => capturedOrder = order)
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.OrderNumber.Should().Be("ORD-001");
        result.Description.Should().Be("Test order");
        result.Status.Should().Be(OrderTrackingApplication.Domain.Enums.OrderStatus.Created);
        capturedOrder.Should().NotBeNull();
        _orderRepository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ThrowsValidationException_WhenOrderNumberAlreadyExists()
    {
        var command = new CreateOrderCommand("ORD-001", "Test order");

        _orderRepository
            .Setup(x => x.ExistsByOrderNumberAsync(command.OrderNumber, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ValidationException>();
        _orderRepository.Verify(x => x.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
