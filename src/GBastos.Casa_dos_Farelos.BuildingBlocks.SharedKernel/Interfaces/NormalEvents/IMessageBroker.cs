namespace GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;

public interface IMessageBroker
{
    Task PublishAsync<T>(
        string topic,
        T message,
        CancellationToken ct);
}