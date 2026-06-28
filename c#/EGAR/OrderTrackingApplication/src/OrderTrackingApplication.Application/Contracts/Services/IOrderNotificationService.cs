using OrderTrackingApplication.Domain.Enums;

namespace OrderTrackingApplication.Application.Contracts.Services;

/// <summary>
/// Сервис real-time уведомлений об изменении статуса заказа
/// </summary>
public interface IOrderNotificationService
{
    /// <summary>
    /// Отправить уведомление подписчикам об изменении статуса заказа
    /// </summary>
    /// <param name="orderId">Идентификатор заказа</param>
    /// <param name="orderNumber">Номер заказа</param>
    /// <param name="previousStatus">Предыдущий статус</param>
    /// <param name="newStatus">Новый статус</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task NotifyStatusChangedAsync(
        Guid orderId,
        string orderNumber,
        OrderStatus previousStatus,
        OrderStatus newStatus,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Подписаться на уведомления об изменении статуса заказов
    /// </summary>
    /// <param name="orderId">Фильтр по идентификатору заказа (необязательно)</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Поток уведомлений</returns>
    IAsyncEnumerable<OrderStatusNotification> SubscribeAsync(
        Guid? orderId = null,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Уведомление об изменении статуса заказа
/// </summary>
/// <param name="OrderId">Идентификатор заказа</param>
/// <param name="OrderNumber">Номер заказа</param>
/// <param name="PreviousStatus">Предыдущий статус</param>
/// <param name="NewStatus">Новый статус</param>
/// <param name="OccurredAt">Время изменения</param>
public sealed record OrderStatusNotification(
    Guid OrderId,
    string OrderNumber,
    OrderStatus PreviousStatus,
    OrderStatus NewStatus,
    DateTime OccurredAt);
