using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.IntegrationEvents;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.DomainEvents;

public sealed class InMemoryEventBus : IEventBus
{
    private readonly IServiceScopeFactory _scopeFactory;

    private readonly ConcurrentDictionary<Type, List<Type>> _handlers = new();

    public InMemoryEventBus(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task PublishAsync<TEvent>(
        TEvent integrationEvent,
        CancellationToken ct = default)
        where TEvent : class, IIntegrationEvent
    {
        if (integrationEvent is null)
            throw new ArgumentNullException(nameof(integrationEvent));

        var eventType = integrationEvent.GetType();

        if (!_handlers.TryGetValue(eventType, out var handlerTypes))
            return;

        using var scope = _scopeFactory.CreateScope();

        var tasks = new List<Task>();

        foreach (var handlerType in handlerTypes)
        {
            var handler = scope.ServiceProvider.GetRequiredService(handlerType);

            var method = handlerType.GetMethod("HandleAsync")
                ?? throw new InvalidOperationException(
                    $"Handler {handlerType.Name} não possui HandleAsync.");

            var task = (Task)method.Invoke(handler, new object[] { integrationEvent, ct })!;
            tasks.Add(task);
        }

        await Task.WhenAll(tasks);
    }

    public void Subscribe<TEvent, THandler>()
        where TEvent : class, IIntegrationEvent
        where THandler : class, IIntegrationEventHandler<TEvent>
    {
        var eventType = typeof(TEvent);
        var handlerType = typeof(THandler);

        _handlers.AddOrUpdate(
            eventType,
            _ => new List<Type> { handlerType },
            (_, handlers) =>
            {
                if (!handlers.Contains(handlerType))
                    handlers.Add(handlerType);

                return handlers;
            });
    }
}