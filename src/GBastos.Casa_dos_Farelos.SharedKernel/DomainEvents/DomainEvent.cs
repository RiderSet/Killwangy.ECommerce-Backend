using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;
using MediatR;

namespace GBastos.Casa_dos_Farelos.SharedKernel.DomainEvents;

public abstract class DomainEvent : IDomainEvent, INotification
{
    public Guid EventId { get; }
    public DateTime OccurredOnUtc { get; }
    public Guid? CorrelationId { get; }
    public Guid? CausationId { get; }
    public Guid AggregateId { get; }
    public string EventType => GetType().Name;

    public virtual int Version => 1;

    public Dictionary<string, string>? Metadata { get; }

    protected DomainEvent(
        Guid aggregateId,
        Guid? correlationId = null,
        Guid? causationId = null,
        Dictionary<string, string>? metadata = null)
    {
        EventId = Guid.NewGuid();
        OccurredOnUtc = DateTime.UtcNow;

        AggregateId = aggregateId;

        CorrelationId = correlationId;
        CausationId = causationId;

        Metadata = metadata;
    }
}