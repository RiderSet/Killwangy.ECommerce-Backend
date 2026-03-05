namespace GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

public interface IMessageBroker
{
    Task PublishAsync<T>(
        string topic,
        T message,
        CancellationToken ct);
}