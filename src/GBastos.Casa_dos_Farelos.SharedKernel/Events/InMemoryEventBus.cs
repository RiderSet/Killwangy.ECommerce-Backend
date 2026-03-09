using GBastos.Casa_dos_Farelos.Domain.Interfaces;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace GBastos.Casa_dos_Farelos.SharedKernel.Events;

public sealed class InMemoryEventBus : IEventBus
{
    private readonly IServiceScopeFactory _scopeFactory;
    private static readonly ConcurrentDictionary<Type, List<Type>> _handlers = new();

    public InMemoryEventBus(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task PublishAsync<TEvent>(
        TEvent integrationEvent,
        CancellationToken ct = default)
        where TEvent : class, IIntegrationEvent
    {
        var eventType = typeof(TEvent);

        if (!_handlers.TryGetValue(eventType, out var handlers))
            return;

        using var scope = _scopeFactory.CreateScope();

        foreach (var handlerType in handlers)
        {
            var handler = scope.ServiceProvider.GetRequiredService(handlerType);

            var method = handlerType.GetMethod("Handle");

            if (method is null)
                continue;

            var task = (Task?)method.Invoke(handler, new object[] { integrationEvent, ct });

            if (task != null)
                await task;
        }
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