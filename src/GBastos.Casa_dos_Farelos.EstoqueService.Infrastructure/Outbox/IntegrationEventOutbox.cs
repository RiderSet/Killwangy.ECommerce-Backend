using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.IntegrationEvents;
using GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Interfaces;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Outbox;

public sealed class IntegrationEventOutbox : IIntegrationEventOutbox
{
    private readonly IOutboxRepository _repository;

    public IntegrationEventOutbox(IOutboxRepository repository)
    {
        _repository = repository;
    }

    public async Task AddAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        if (integrationEvent is null)
            throw new ArgumentNullException(nameof(integrationEvent));

        await _repository.AddAsync(integrationEvent, cancellationToken);
    }

    public async Task<IEnumerable<IIntegrationEvent>> GetPendingAsync(CancellationToken cancellationToken = default)
    {
        var events = await _repository.GetPendingAsync(cancellationToken);

        return events;
    }

    public async Task MarkAsProcessedAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        if (eventId == Guid.Empty)
            throw new ArgumentException("EventId inválido", nameof(eventId));

        await _repository.MarkAsProcessedAsync(eventId, cancellationToken);
    }
}