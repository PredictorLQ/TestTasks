using System.Diagnostics;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderTrackingApplication.Application.Contracts.Messaging;
using OrderTrackingApplication.Infrastructure.Telemetry;
using Polly;
using Polly.CircuitBreaker;

namespace OrderTrackingApplication.Infrastructure.Messaging;

public sealed class KafkaEventPublisher : IEventPublisher, IDisposable
{
    private readonly IProducer<string, string> _producer;
    private readonly KafkaOptions _options;
    private readonly ResiliencePipeline _pipeline;
    private readonly ILogger<KafkaEventPublisher> _logger;

    public KafkaEventPublisher(
        IOptions<KafkaOptions> options,
        ILogger<KafkaEventPublisher> logger)
    {
        _options = options.Value;
        _logger = logger;

        var config = new ProducerConfig
        {
            BootstrapServers = _options.BootstrapServers,
            Acks = Acks.All,
            EnableIdempotence = true,
            MessageSendMaxRetries = 3
        };

        _producer = new ProducerBuilder<string, string>(config).Build();

        _pipeline = new ResiliencePipelineBuilder()
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                FailureRatio = 1.0,
                MinimumThroughput = _options.CircuitBreakerFailures,
                SamplingDuration = TimeSpan.FromSeconds(_options.CircuitBreakerDurationSeconds),
                BreakDuration = TimeSpan.FromSeconds(_options.CircuitBreakerDurationSeconds),
                OnOpened = args =>
                {
                    _logger.LogWarning(
                        "Kafka circuit breaker открыт на {Duration}s. Причина: {Exception}",
                        args.BreakDuration.TotalSeconds,
                        args.Outcome.Exception?.Message);
                    return ValueTask.CompletedTask;
                },
                OnClosed = _ =>
                {
                    _logger.LogInformation("Kafka circuit breaker закрыт.");
                    return ValueTask.CompletedTask;
                }
            })
            .Build();
    }

    public async Task PublishAsync(
        string topic,
        string key,
        string payload,
        CancellationToken cancellationToken = default)
    {
        using var activity = OrderTrackingTelemetry.ActivitySource.StartActivity(
            "kafka.publish",
            ActivityKind.Producer);

        activity?.SetTag("messaging.system", "kafka");
        activity?.SetTag("messaging.destination.name", topic);
        activity?.SetTag("messaging.message.id", key);

        await _pipeline.ExecuteAsync(async token =>
        {
            var result = await _producer.ProduceAsync(
                topic,
                new Message<string, string> { Key = key, Value = payload },
                token);

            _logger.LogInformation(
                "Событие опубликовано в Kafka. Topic: {Topic}, Key: {Key}, Partition: {Partition}, Offset: {Offset}",
                result.Topic,
                key,
                result.Partition.Value,
                result.Offset.Value);
        }, cancellationToken);
    }

    public void Dispose()
    {
        _producer.Flush(TimeSpan.FromSeconds(5));
        _producer.Dispose();
    }
}
