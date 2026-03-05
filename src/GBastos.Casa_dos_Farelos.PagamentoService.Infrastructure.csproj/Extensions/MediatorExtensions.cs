using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Common;
using GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Persistence.Context;
using MassTransit.Mediator;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Extensions;

public static class MediatorExtensions
{
    public static async Task PublishDomainEventsAsync(
        this IMediator mediator,
        PagamentoDbContext context)
    {
        var domainEntities = context.ChangeTracker
            .Entries<AggregateRoot>()
            .Where(x => x.Entity.DomainEvents.Any())
            .Select(x => x.Entity)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.DomainEvents)
            .ToList();

        domainEntities.ForEach(entity => entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent);
        }
    }
}