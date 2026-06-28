using FluentAssertions;
using OrderTrackingApplication.Domain.Entities;
using OrderTrackingApplication.Domain.Enums;

namespace OrderTrackingApplication.UnitTests.Domain;

public class OrderTests
{
    [Fact]
    public void UpdateStatus_WhenStatusChanges_UpdatesStatusAndUpdatedAt()
    {
        var order = new Order
        {
            OrderNumber = "ORD-001",
            Description = "Test",
            Status = OrderStatus.Created,
            UpdatedAt = DateTime.UtcNow.AddHours(-1)
        };

        order.UpdateStatus(OrderStatus.Sent);

        order.Status.Should().Be(OrderStatus.Sent);
        order.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }

    [Fact]
    public void UpdateStatus_WhenSameStatus_DoesNotChangeUpdatedAt()
    {
        var updatedAt = DateTime.UtcNow.AddHours(-1);
        var order = new Order
        {
            Status = OrderStatus.Created,
            UpdatedAt = updatedAt
        };

        order.UpdateStatus(OrderStatus.Created);

        order.Status.Should().Be(OrderStatus.Created);
        order.UpdatedAt.Should().Be(updatedAt);
    }
}
