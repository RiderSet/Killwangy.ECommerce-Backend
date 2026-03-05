using MediatR;

public abstract class AggregateRoot<TId>
{
    private readonly List<INotification> _domainEvents = new();

    public TId Id { get; protected set; }

    public IReadOnlyCollection<INotification> DomainEvents
        => _domainEvents.AsReadOnly();

    protected AggregateRoot(TId id)
    {
        Id = id;
    }

    protected void AddDomainEvent(INotification domainEvent)
        => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents()
        => _domainEvents.Clear();

    protected abstract void ValidateInvariants();
}