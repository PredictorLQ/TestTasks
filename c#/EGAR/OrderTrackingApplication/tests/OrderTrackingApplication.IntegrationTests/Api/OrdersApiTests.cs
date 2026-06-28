using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using OrderTrackingApplication.Domain.Dtos;
using OrderTrackingApplication.Domain.Enums;
using OrderTrackingApplication.Infrastructure.Data;
using OrderTrackingApplication.IntegrationTests.Infrastructure;

namespace OrderTrackingApplication.IntegrationTests.Api;

public sealed class OrdersApiTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public OrdersApiTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    public Task InitializeAsync() => _factory.ResetDatabaseAsync();

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task CreateOrder_ReturnsCreatedOrder()
    {
        var response = await _client.PostAsJsonAsync("/api/orders", new
        {
            orderNumber = "ORD-INT-001",
            description = "Integration test order"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var order = await response.Content.ReadFromJsonAsync<OrderDto>(JsonOptions);
        order.Should().NotBeNull();
        order!.OrderNumber.Should().Be("ORD-INT-001");
        order.Status.Should().Be(OrderStatus.Created);
    }

    [Fact]
    public async Task GetAllOrders_ReturnsCreatedOrders()
    {
        await _client.PostAsJsonAsync("/api/orders", new
        {
            orderNumber = "ORD-INT-002",
            description = "Order 2"
        });

        var response = await _client.GetAsync("/api/orders");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var orders = await response.Content.ReadFromJsonAsync<List<OrderDto>>(JsonOptions);
        orders.Should().NotBeNull();
        orders!.Should().ContainSingle(x => x.OrderNumber == "ORD-INT-002");
    }

    [Fact]
    public async Task GetOrderById_ReturnsOrder_WhenExists()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/orders", new
        {
            orderNumber = "ORD-INT-003",
            description = "Order 3"
        });
        var created = await createResponse.Content.ReadFromJsonAsync<OrderDto>(JsonOptions);

        var response = await _client.GetAsync($"/api/orders/{created!.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var order = await response.Content.ReadFromJsonAsync<OrderDto>(JsonOptions);
        order!.Id.Should().Be(created.Id);
    }

    [Fact]
    public async Task GetOrderById_ReturnsNotFound_WhenOrderDoesNotExist()
    {
        var response = await _client.GetAsync($"/api/orders/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetOrderByNumber_ReturnsOrder_WhenExists()
    {
        await _client.PostAsJsonAsync("/api/orders", new
        {
            orderNumber = "ORD-INT-004",
            description = "Order 4"
        });

        var response = await _client.GetAsync("/api/orders/by-number/ORD-INT-004");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var order = await response.Content.ReadFromJsonAsync<OrderDto>(JsonOptions);
        order!.OrderNumber.Should().Be("ORD-INT-004");
    }

    [Fact]
    public async Task UpdateStatus_ReturnsUpdatedOrder_AndCreatesOutboxMessage()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/orders", new
        {
            orderNumber = "ORD-INT-005",
            description = "Order 5"
        });
        var created = await createResponse.Content.ReadFromJsonAsync<OrderDto>(JsonOptions);

        var response = await _client.PatchAsJsonAsync(
            $"/api/orders/{created!.Id}/status",
            new { status = "Sent" });

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var updated = await response.Content.ReadFromJsonAsync<OrderDto>(JsonOptions);
        updated!.Status.Should().Be(OrderStatus.Sent);

        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
        var outboxMessages = dbContext.OutboxMessages.ToList();

        outboxMessages.Should().ContainSingle();
        outboxMessages[0].EventType.Should().Be("OrderStatusChangedEvent");
        outboxMessages[0].ProcessedAt.Should().BeNull();
    }

    [Fact]
    public async Task CreateOrder_ReturnsBadRequest_WhenOrderNumberIsEmpty()
    {
        var response = await _client.PostAsJsonAsync("/api/orders", new
        {
            orderNumber = "",
            description = "Invalid order"
        });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
