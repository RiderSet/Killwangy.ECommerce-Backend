using MediatR;
namespace GBastos.Casa_dos_Farelos.Shared.Interfaces;

public interface IIntegrationEvent : INotification
{
    Guid Id { get; }
    DateTime OccurredOnUtc { get; }
    string EventType { get; }
    int Version { get; }
}