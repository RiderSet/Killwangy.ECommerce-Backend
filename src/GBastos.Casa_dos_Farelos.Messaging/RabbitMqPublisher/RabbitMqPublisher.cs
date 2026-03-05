using RabbitMQ.Client;
using System.Text;

namespace GBastos.Casa_dos_Farelos.Messaging.RabbitMqPublisher;

public class RabbitMqPublisher
{
    private readonly IChannel _channel;

    public RabbitMqPublisher(RabbitMqConnection connection)
        => _channel = connection.Channel;

    public async Task PublishAsync(
        string queueName,
        string message,
        Guid messageId,
        string eventType,
        DateTime occurredOnUtc,
        int version,
        CancellationToken ct = default)
    {
        var body = Encoding.UTF8.GetBytes(message);

        var props = new BasicProperties
        {
            DeliveryMode = DeliveryModes.Persistent,
            MessageId = messageId.ToString(),
            Type = eventType,
            Timestamp = new AmqpTimestamp(
                new DateTimeOffset(occurredOnUtc).ToUnixTimeSeconds()),
            ContentType = "application/json",
            Headers = new Dictionary<string, object?>
            {
                { "version", version }
            }
        };

        await _channel.BasicPublishAsync(
            exchange: "",
            routingKey: queueName,
            mandatory: false,
            basicProperties: props,
            body: body,
            cancellationToken: ct);
    }
}