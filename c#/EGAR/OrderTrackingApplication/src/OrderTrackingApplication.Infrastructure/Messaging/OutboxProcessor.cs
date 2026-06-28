using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderTrackingApplication.Application.Contracts.Messaging;
using OrderTrackingApplication.Application.Contracts.Repositories;
using OrderTrackingApplication.Infrastructure.Telemetry;

namespace OrderTrackingApplication.Infrastructure.Messaging;

public sealed class OutboxProcessor(
    IServiceScopeFactory scopeFactory,
    IOptions<KafkaOptions> kafkaOptions,
    ILogger<OutboxProcessor> logger) : BackgroundService
{
    private readonly KafkaOptions _kafkaOptions = kafkaOptions.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Outbox processor запущен.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessPendingMessagesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при обработке outbox-сообщений.");
            }

            await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
        }
    }

    private async Task ProcessPendingMessagesAsync(CancellationToken cancellationToken)
    {
        using var activity = OrderTrackingTelemetry.ActivitySource.StartActivity(
            "outbox.process",
            ActivityKind.Internal);

        using var scope = scopeFactory.CreateScope();
        var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();
        var eventPublisher = scope.ServiceProvider.GetRequiredService<IEventPublisher>();

        var messages = await outboxRepository.GetPendingAsync(20, cancellationToken);
        activity?.SetTag("outbox.batch.size", messages.Count);

        foreach (var message in messages)
        {
            try
            {
                await eventPublisher.PublishAsync(
                    _kafkaOptions.OrderStatusTopic,
                    message.IdempotencyKey,
                    message.Payload,
                    cancellationToken);

                await outboxRepository.MarkAsProcessedAsync(message.Id, cancellationToken);

                logger.LogInformation(
                    "Outbox-сообщение обработано. MessageId: {MessageId}, IdempotencyKey: {IdempotencyKey}",
                    message.Id,
                    message.IdempotencyKey);
            }
            catch (Exception ex)
            {
                logger.LogWarning(
                    ex,
                    "Не удалось опубликовать outbox-сообщение. MessageId: {MessageId}",
                    message.Id);

                await outboxRepository.MarkAsFailedAsync(message.Id, ex.Message, cancellationToken);
            }
        }
    }
}
