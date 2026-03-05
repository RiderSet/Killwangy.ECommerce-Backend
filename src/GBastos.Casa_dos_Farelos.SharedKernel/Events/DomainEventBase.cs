using MediatR;

namespace GBastos.Casa_dos_Farelos.SharedKernel.Events;

public abstract record DomainEventBase : INotification
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
}