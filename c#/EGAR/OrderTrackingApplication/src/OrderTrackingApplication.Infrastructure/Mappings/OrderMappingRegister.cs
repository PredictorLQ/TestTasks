using Mapster;
using OrderTrackingApplication.Domain.Dtos;
using OrderTrackingApplication.Domain.Entities;

namespace OrderTrackingApplication.Infrastructure.Mappings;

public static class OrderMappingRegister
{
    public static void Configure()
    {
        TypeAdapterConfig<Order, OrderDto>.NewConfig();
    }
}
