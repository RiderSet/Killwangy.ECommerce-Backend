using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Events;

namespace GBastos.Casa_dos_Farelos.PedidoService.Domain.Common;

public abstract class BaseEntity
{

    public Guid Id { get; protected set; }
    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents;

    protected void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}