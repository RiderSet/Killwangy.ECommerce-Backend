using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.IntegrationEvents;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;
using GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Messaging.EventBus;

public sealed class RabbitMqEventBus : IEventBus, IAsyncDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly IEventSerializer _serializer;
    private readonly string _exchangeName;

    public RabbitMqEventBus(
        IConfiguration config,
        IEventSerializer serializer)
    {
        _serializer = serializer;

        var factory = new ConnectionFactory
        {
            HostName = config["RabbitMQ:Host"] ?? "localhost",
            UserName = config["RabbitMQ:User"] ?? "guest",
            Password = config["RabbitMQ:Password"] ?? "guest"
        };

        _connection = factory
            .CreateConnectionAsync()
            .GetAwaiter()
            .GetResult();

        _channel = _connection
            .CreateChannelAsync()
            .GetAwaiter()
            .GetResult();

        _exchangeName = config["RabbitMQ:Exchange"] ?? "casadosfarelos.events";

        _channel.ExchangeDeclareAsync(
            exchange: _exchangeName,
            type: ExchangeType.Topic,
            durable: true)
            .GetAwaiter()
            .GetResult();
    }

    public async Task PublishAsync<TEvent>(
        TEvent integrationEvent,
        CancellationToken ct = default)
        where TEvent : class, IIntegrationEvent
    {
        var routingKey = integrationEvent.EventType;

        var message = _serializer.Serialize(integrationEvent);

        var body = Encoding.UTF8.GetBytes(message);

        var properties = new BasicProperties
        {
            Persistent = true,
            MessageId = integrationEvent.Id.ToString(),
            Timestamp = new AmqpTimestamp(
                new DateTimeOffset(integrationEvent.OccurredOnUtc)
                    .ToUnixTimeSeconds())
        };

        await _channel.BasicPublishAsync(
            exchange: _exchangeName,
            routingKey: routingKey,
            mandatory: false,
            basicProperties: properties,
            body: body,
            cancellationToken: ct);
    }

    public async Task SubscribeAsync<TEvent, THandler>()
        where TEvent : class, IIntegrationEvent
        where THandler : class, IIntegrationEventHandler<TEvent>, new()
    {
        var queueName = typeof(TEvent).Name;

        await _channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        await _channel.QueueBindAsync(
            queue: queueName,
            exchange: _exchangeName,
            routingKey: queueName);

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (sender, args) =>
        {
            var body = args.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            var @event = _serializer.Deserialize<TEvent>(json);
            var handler = new THandler();
            await handler.Handle(@event, CancellationToken.None);
            await _channel.BasicAckAsync(args.DeliveryTag, false);
        };

        await _channel.BasicConsumeAsync(
            queue: queueName,
            autoAck: false,
            consumer: consumer);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel != null)
            await _channel.CloseAsync();

        if (_connection != null)
            await _connection.CloseAsync();
    }

    void IEventBus.Subscribe<TEvent, THandler>()
    {
        throw new NotImplementedException();
    }
}