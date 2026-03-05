namespace GBastos.Casa_dos_Farelos.Messaging.Abstractions;

public interface IProcessedEventRepository
{
    Task<bool> ExistsAsync(Guid eventId, CancellationToken ct);
    Task AddAsync(Guid eventId, CancellationToken ct);
}
