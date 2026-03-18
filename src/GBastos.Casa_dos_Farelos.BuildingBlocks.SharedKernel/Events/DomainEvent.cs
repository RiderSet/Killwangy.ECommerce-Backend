using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;
using MediatR;

namespace GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Events;

public abstract record DomainEvent : IDomainEvent, INotification
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
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
        AggregateId = aggregateId;
        CorrelationId = correlationId;
        CausationId = causationId;
        Metadata = metadata;
    }
}