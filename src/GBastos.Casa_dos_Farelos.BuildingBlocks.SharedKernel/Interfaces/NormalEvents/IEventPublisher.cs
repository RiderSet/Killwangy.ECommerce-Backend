namespace GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;

public interface IEventPublisher
{
    Task PublishAsync(
        string routingKey,
        string payload,
        Guid messageId,
        string eventType,
        DateTime occurredOnUtc,
        int version,
        CancellationToken cancellationToken = default);
}