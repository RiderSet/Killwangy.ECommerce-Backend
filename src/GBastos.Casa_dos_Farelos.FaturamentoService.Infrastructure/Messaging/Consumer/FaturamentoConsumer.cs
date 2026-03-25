using GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Events.Consume;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Infrastructure.Messaging.Consumer;

public class FaturamentoConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IServiceProvider _provider;

    public FaturamentoConsumer(
        IConnection connection,
        IServiceProvider provider)
    {
        _connection = connection;
        _provider = provider;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        var channel = await _connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(
            exchange: "faturamento.events",
            type: ExchangeType.Topic,
            durable: true);

        await channel.QueueDeclareAsync(
            queue: "faturamento.queue",
            durable: true,
            exclusive: false,
            autoDelete: false);

        await channel.QueueBindAsync(
            queue: "faturamento.queue",
            exchange: "faturamento.events",
            routingKey: "pedido.confirmado");

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (sender, args) =>
        {
            using var scope = _provider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var body = args.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);

            var evento = JsonSerializer.Deserialize<PedidoConfirmadoEvent>(json);

            if (evento is not null)
            {
                await mediator.Publish(evento, stoppingToken);
            }

            await channel.BasicAckAsync(args.DeliveryTag, false);
        };

        await channel.BasicConsumeAsync(
            queue: "faturamento.queue",
            autoAck: false,
            consumer: consumer);
    }
}