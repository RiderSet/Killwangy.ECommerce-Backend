namespace GBastos.Casa_dos_Farelos.Gateway.DTOs;

public sealed record PublishEventRequest(
    string EventType,
    string Payload,
    DateTime OccurredOnUtc
);