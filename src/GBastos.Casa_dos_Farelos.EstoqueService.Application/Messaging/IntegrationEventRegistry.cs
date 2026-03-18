using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.IntegrationEvents.General;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.IntegrationEvents;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;
using GBastos.Casa_dos_Farelos.EstoqueService.Application.IntegrationEvents;
using GBastos.Casa_dos_Farelos.EstoqueService.Domain.Events;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Messaging;

public sealed class IntegrationEventRegistry : IIntegrationEventRegistry
{
    private static readonly IReadOnlyDictionary<Type, Func<IDomainEvent, IntegrationEvent>> _map =
        new Dictionary<Type, Func<IDomainEvent, IntegrationEvent>>
        {
            {
                typeof(ProdutoCriadoEvent),
                Create<ProdutoCriadoEvent>(evt =>
                    new ProdutoCriadoIntegrationEvent(evt.ProdutoId, evt.Preco)
                    {
                        Nome = evt.Nome
                    })
            },

            {
                typeof(EstoqueBaixadoDomainEvent),
                Create<EstoqueBaixadoDomainEvent>(evt =>
                    new EstoqueBaixadoIntegrationEvent(
                        evt.ProdutoId,
                        evt.NomeProduto,
                        evt.Quantidade))
            }
        };

    private static Func<IDomainEvent, IntegrationEvent> Create<T>(
        Func<T, IntegrationEvent> factory)
        where T : IDomainEvent
    {
        return e =>
        {
            if (e is not T typed)
                throw new InvalidOperationException(
                    $"Tipo inválido para o mapeamento. Esperado {typeof(T).Name}");

            return factory(typed);
        };
    }

    public IntegrationEvent? Map(IDomainEvent domainEvent)
    {
        if (domainEvent is null)
            return null;

        return _map.TryGetValue(domainEvent.GetType(), out var factory)
            ? factory(domainEvent)
            : null;
    }
}