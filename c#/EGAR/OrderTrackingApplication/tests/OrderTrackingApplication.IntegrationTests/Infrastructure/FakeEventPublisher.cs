using OrderTrackingApplication.Application.Contracts.Messaging;

namespace OrderTrackingApplication.IntegrationTests.Infrastructure;

public sealed class FakeEventPublisher : IEventPublisher
{
    public List<PublishedMessage> Messages { get; } = [];

    public Task PublishAsync(string topic, string key, string payload, CancellationToken cancellationToken = default)
    {
        Messages.Add(new PublishedMessage(topic, key, payload));
        return Task.CompletedTask;
    }
}

public sealed record PublishedMessage(string Topic, string Key, string Payload);
