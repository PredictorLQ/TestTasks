using OrderTrackingApplication.Domain.Enums;

namespace OrderTrackingApplication.Api.Bindings;

/// <summary>
/// Модель запроса для создания заказа
/// </summary>
/// <param name="OrderNumber">Уникальный номер заказа</param>
/// <param name="Description">Описание заказа</param>
public sealed record CreateOrderBinding(
    string OrderNumber,
    string Description);

/// <summary>
/// Модель запроса для изменения статуса заказа
/// </summary>
/// <param name="Status">Новый статус заказа</param>
public sealed record UpdateOrderStatusBinding(
    OrderStatus Status);
