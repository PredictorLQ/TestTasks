using OrderTrackingApplication.Domain.Entities;
using OrderTrackingApplication.Domain.Enums;

namespace OrderTrackingApplication.Application.Contracts.Repositories;

/// <summary>
/// Репозиторий для работы с заказами
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    /// Получить заказ по идентификатору (без отслеживания изменений)
    /// </summary>
    /// <param name="id">Идентификатор заказа</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить заказ по идентификатору для обновления (с отслеживанием изменений)
    /// </summary>
    /// <param name="id">Идентификатор заказа</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task<Order?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить заказ по номеру
    /// </summary>
    /// <param name="orderNumber">Номер заказа</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить список всех заказов
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Проверить существование заказа с указанным номером
    /// </summary>
    /// <param name="orderNumber">Номер заказа</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task<bool> ExistsByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавить новый заказ
    /// </summary>
    /// <param name="order">Сущность заказа</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task AddAsync(Order order, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить существующий заказ
    /// </summary>
    /// <param name="order">Сущность заказа</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task UpdateAsync(Order order, CancellationToken cancellationToken = default);

    /// <summary>
    /// Сохранить изменения в базе данных
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
