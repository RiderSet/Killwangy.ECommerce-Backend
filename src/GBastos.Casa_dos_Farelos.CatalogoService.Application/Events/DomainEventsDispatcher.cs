using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Abstractions;
using GBastos.Casa_dos_Farelos.CatalogoService.Infrastructure.Persistence;
using MassTransit.Mediator;

namespace GBastos.Casa_dos_Farelos.CatalogoService.Application.Events;

public static class DomainEventsDispatcher
{
    public static async Task DispatchAsync(
        IMediator mediator,
        CatalogoDbContext context)
    {
        var aggregates = context.ChangeTracker
            .Entries<AggregateRoot<Guid>>()
            .Where(x => x.Entity.DomainEvents.Any())
            .Select(x => x.Entity)
            .ToList();

        var domainEvents = aggregates
            .SelectMany(x => x.DomainEvents)
            .ToList();

        aggregates.ForEach(a => a.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent);
        }
    }
}