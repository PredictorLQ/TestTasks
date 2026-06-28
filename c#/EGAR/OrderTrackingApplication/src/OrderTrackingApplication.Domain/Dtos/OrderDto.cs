using OrderTrackingApplication.Domain.Enums;

namespace OrderTrackingApplication.Domain.Dtos;

/// <summary>
/// DTO заказа
/// </summary>
/// <param name="Id">Идентификатор заказа</param>
/// <param name="OrderNumber">Номер заказа</param>
/// <param name="Description">Описание заказа</param>
/// <param name="Status">Текущий статус заказа</param>
/// <param name="CreatedAt">Дата создания</param>
/// <param name="UpdatedAt">Дата последнего изменения</param>
public sealed record OrderDto(
    Guid Id,
    string OrderNumber,
    string Description,
    OrderStatus Status,
    DateTime CreatedAt,
    DateTime UpdatedAt);
