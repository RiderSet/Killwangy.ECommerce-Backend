using GBastos.Casa_dos_Farelos.Domain.Interfaces;
using GBastos.Casa_dos_Farelos.Messaging.Abstractions;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace GBastos.Casa_dos_Farelos.Messaging.RabbitMqPublisher;

public sealed class RabbitMqEventBus : IEventBus, IAsyncDisposable
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IEventSerializer _serializer;
    private readonly RabbitMqOptions _options;

    private IConnection? _connection;
    private IChannel? _channel;

    private bool _initialized;
    private readonly Dictionary<string, Type> _eventTypes = new();

    public RabbitMqEventBus(
        IServiceScopeFactory scopeFactory,
        IEventSerializer serializer,
        RabbitMqOptions options)
    {
        _scopeFactory = scopeFactory;
        _serializer = serializer;
        _options = options;
    }

    private async Task EnsureInitializedAsync()
    {
        if (_initialized)
            return;

        var factory = new ConnectionFactory
        {
            HostName = _options.Host,
            Port = _options.Port,
            UserName = _options.Username,
            Password = _options.Password
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.ExchangeDeclareAsync(
            exchange: _options.ExchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false);

        _initialized = true;
    }

    public async Task PublishAsync<T>(
        T @event,
        CancellationToken ct = default)
        where T : class, IIntegrationEvent
    {
        await EnsureInitializedAsync();

        var routingKey = @event.EventType;
        var payload = _serializer.Serialize(@event);
        var body = Encoding.UTF8.GetBytes(payload);

        var properties = new BasicProperties
        {
            Persistent = true,
            MessageId = @event.Id.ToString(),
            Type = @event.EventType
        };

        await _channel!.BasicPublishAsync(
            exchange: _options.ExchangeName,
            routingKey: routingKey,
            mandatory: false,
            basicProperties: properties,
            body: body,
            cancellationToken: ct);
    }

    public async void Subscribe<T, THandler>()
        where T : class, IIntegrationEvent
        where THandler : class, IIntegrationEventHandler<T>
    {
        await EnsureInitializedAsync();

        var eventName = typeof(T).Name;
        var queueName = $"{_options.QueuePrefix}.{eventName}";

        _eventTypes[eventName] = typeof(T);

        await _channel!.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        await _channel.QueueBindAsync(
            queue: queueName,
            exchange: _options.ExchangeName,
            routingKey: eventName);

        var consumer = new RabbitMqConsumer(
            _channel,
            ea => ProcessMessage(ea, typeof(THandler)));

        await _channel.BasicConsumeAsync(
            queue: queueName,
            autoAck: false,
            consumer: consumer);
    }

    private async Task ProcessMessage(
        BasicDeliverEventArgs ea,
        Type handlerType)
    {
        var eventName = ea.BasicProperties.Type;

        if (string.IsNullOrWhiteSpace(eventName))
        {
            await _channel!.BasicNackAsync(ea.DeliveryTag, false, false);
            return;
        }

        var payload = Encoding.UTF8.GetString(ea.Body.Span);

        if (!_eventTypes.TryGetValue(eventName, out var eventType))
        {
            await _channel!.BasicNackAsync(ea.DeliveryTag, false, false);
            return;
        }

        using var scope = _scopeFactory.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService(handlerType);

        var @event = _serializer.Deserialize(
            payload,
            eventType.AssemblyQualifiedName!);

        try
        {
            var method = handlerType.GetMethod("Handle");

            await (Task)method!.Invoke(
                handler,
                new object[] { @event, CancellationToken.None })!;

            await _channel!.BasicAckAsync(ea.DeliveryTag, false);
        }
        catch
        {
            await _channel!.BasicNackAsync(ea.DeliveryTag, false, false);
        }
    }

    public async ValueTask DisposeAsync()
    {

        if (_channel is not null)
            await _channel.CloseAsync();

        if (_connection is not null)
            await _connection.CloseAsync();
    }

    public Task Publish<T>(T @event, CancellationToken ct = default) where T : class
    {
        throw new NotImplementedException();
    }

    public Task Publish(object @event, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task PublishAsync(IIntegrationEvent integrationEvent, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}