using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;
using MediatR;

namespace GBastos.Casa_dos_Farelos.AuthService.Domain.Common;

public abstract class BaseEntity : IEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();

    private readonly List<INotification> _domainEvents = new();
    public IReadOnlyCollection<INotification> DomainEvents
        => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(INotification domainEvent)
        => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents()
        => _domainEvents.Clear();
}