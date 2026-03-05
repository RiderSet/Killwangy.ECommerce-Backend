using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace GBastos.Casa_dos_Farelos.Messaging.RabbitMqPublisher;

internal sealed class RabbitMqConsumer : AsyncDefaultBasicConsumer
{
    private readonly Func<BasicDeliverEventArgs, Task> _handler;

    public RabbitMqConsumer(
        IChannel channel,
        Func<BasicDeliverEventArgs, Task> handler)
        : base(channel)
    {
        _handler = handler;
    }

    public override async Task HandleBasicDeliverAsync(
        string consumerTag,
        ulong deliveryTag,
        bool redelivered,
        string exchange,
        string routingKey,
        IReadOnlyBasicProperties properties,
        ReadOnlyMemory<byte> body,
        CancellationToken cancellationToken = default)
    {
        var ea = new BasicDeliverEventArgs(
            consumerTag,
            deliveryTag,
            redelivered,
            exchange,
            routingKey,
            properties,
            body);

        await _handler(ea);
    }
}