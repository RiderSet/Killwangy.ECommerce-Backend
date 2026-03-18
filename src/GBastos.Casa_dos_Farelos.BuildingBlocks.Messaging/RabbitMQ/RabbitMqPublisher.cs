using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.BuildingBlocks.Messaging.RabbitMQ;

public sealed class RabbitMqPublisher : IEventPublisher
{
    private readonly IConnection _connection;

    public RabbitMqPublisher(IConnection connection)
    {
        _connection = connection;
    }

    public async Task PublishAsync(
        string routingKey,
        string payload,
        Guid messageId,
        string eventType,
        DateTime occurredOnUtc,
        int version,
        CancellationToken cancellationToken = default)
    {
        await using var channel = await _connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(
            exchange: "casa_dos_farelos",
            type: ExchangeType.Topic,
            durable: true,
            cancellationToken: cancellationToken
        );

        var envelope = new
        {
            Id = messageId,
            Type = eventType,
            OccurredOnUtc = occurredOnUtc,
            Version = version,
            Payload = JsonSerializer.Deserialize<object>(payload)
        };

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(envelope));

        var properties = new BasicProperties
        {
            Persistent = true,
            MessageId = messageId.ToString(),
            Type = eventType,
            Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
        };

        await channel.BasicPublishAsync(
            exchange: "casa_dos_farelos",
            routingKey: routingKey,
            mandatory: false,
            basicProperties: properties,
            body: body,
            cancellationToken: cancellationToken
        );
    }
}