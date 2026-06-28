namespace OrderTrackingApplication.Application.Contracts.Messaging;

/// <summary>
/// Публикатор событий во внешнюю очередь сообщений (Kafka)
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// Опубликовать событие в указанный топик
    /// </summary>
    /// <param name="topic">Имя топика</param>
    /// <param name="key">Ключ сообщения (для идемпотентности и партиционирования)</param>
    /// <param name="payload">Тело сообщения в формате JSON</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task PublishAsync(string topic, string key, string payload, CancellationToken cancellationToken = default);
}
