using OrderTrackingApplication.Domain.Enums;

namespace OrderTrackingApplication.Domain.Entities;

public class Order : Entity
{
    public string OrderNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public OrderStatus Status { get; set; } = OrderStatus.Created;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public void UpdateStatus(OrderStatus newStatus)
    {
        if (Status == newStatus)
        {
            return;
        }

        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;
    }
}
