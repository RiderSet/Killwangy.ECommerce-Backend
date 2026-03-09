using MediatR;

namespace GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

public interface IDomainEvent : INotification
{
    Guid EventId { get; }
    DateTime OccurredOnUtc { get; }
    Guid AggregateId { get; }
    string EventType { get; }
}