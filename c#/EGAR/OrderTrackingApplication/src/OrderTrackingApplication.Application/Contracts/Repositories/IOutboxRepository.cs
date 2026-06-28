using OrderTrackingApplication.Domain.Entities;

namespace OrderTrackingApplication.Application.Contracts.Repositories;

/// <summary>
/// Репозиторий для работы с outbox-сообщениями (Transactional Outbox)
/// </summary>
public interface IOutboxRepository
{
    /// <summary>
    /// Добавить сообщение в outbox
    /// </summary>
    /// <param name="message">Outbox-сообщение</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task AddAsync(OutboxMessage message, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить необработанные outbox-сообщения
    /// </summary>
    /// <param name="batchSize">Максимальное количество сообщений в пакете</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task<IReadOnlyList<OutboxMessage>> GetPendingAsync(int batchSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Пометить сообщение как успешно обработанное
    /// </summary>
    /// <param name="messageId">Идентификатор сообщения</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task MarkAsProcessedAsync(Guid messageId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Пометить сообщение как неудачно обработанное и увеличить счётчик повторов
    /// </summary>
    /// <param name="messageId">Идентификатор сообщения</param>
    /// <param name="error">Текст ошибки</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task MarkAsFailedAsync(Guid messageId, string error, CancellationToken cancellationToken = default);

    /// <summary>
    /// Сохранить изменения в базе данных
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
