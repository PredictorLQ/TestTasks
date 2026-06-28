using FluentAssertions;
using Moq;
using OrderTrackingApplication.Application.Contracts.Repositories;
using OrderTrackingApplication.Application.Exceptions;
using OrderTrackingApplication.Application.Queries.Orders.GetOrderById;
using OrderTrackingApplication.Domain.Entities;
using OrderTrackingApplication.Domain.Enums;

namespace OrderTrackingApplication.UnitTests.Queries;

public class GetOrderByIdQueryHandlerTests : UnitTestBase
{
    private readonly Mock<IOrderRepository> _orderRepository = new();
    private readonly GetOrderByIdQuery.GetOrderByIdQueryHandle _handler;

    public GetOrderByIdQueryHandlerTests()
    {
        _handler = new GetOrderByIdQuery.GetOrderByIdQueryHandle(_orderRepository.Object);
    }

    [Fact]
    public async Task Handle_ReturnsOrderDto_WhenOrderExists()
    {
        var orderId = Guid.NewGuid();
        var order = new Order
        {
            Id = orderId,
            OrderNumber = "ORD-001",
            Description = "Test",
            Status = OrderStatus.Created
        };

        _orderRepository
            .Setup(x => x.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        var result = await _handler.Handle(new GetOrderByIdQuery(orderId), CancellationToken.None);

        result.Id.Should().Be(orderId);
        result.OrderNumber.Should().Be("ORD-001");
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenOrderDoesNotExist()
    {
        var orderId = Guid.NewGuid();

        _orderRepository
            .Setup(x => x.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Order?)null);

        var act = () => _handler.Handle(new GetOrderByIdQuery(orderId), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
