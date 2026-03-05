using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Messaging.Abstractions;

public abstract class BaseEntity : IEntity
{
    private readonly List<INotification> _domainEvents = new();

    public Guid Id { get; protected set; } = Guid.NewGuid();

    public IReadOnlyCollection<INotification> DomainEvents
        => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(INotification domainEvent)
        => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents()
        => _domainEvents.Clear();

    public object GetId() => Id;
}