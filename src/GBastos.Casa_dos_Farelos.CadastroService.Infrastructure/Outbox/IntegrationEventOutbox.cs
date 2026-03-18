using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.IntegrationEvents.Pagamentos;
using GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Interfaces;

namespace GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Outbox;

public sealed class IntegrationEventOutbox : IIntegrationEventOutbox
{
    private readonly IOutboxRepository _repository;

    public IntegrationEventOutbox(IOutboxRepository repository)
    {
        _repository = repository;
    }

    public async Task AddAsync(
        PagamentoAprovadoIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        var message = OutboxMessage.CreateIntegrationEvent(integrationEvent);

        await _repository.AddAsync(message, cancellationToken);
    }

    public Task<List<OutboxMessage>> GetPendingAsync(
        int batchSize,
        CancellationToken cancellationToken = default)
    {
        return _repository.GetPendingAsync(batchSize, cancellationToken);
    }

    public Task LockAsync(
        OutboxMessage message,
        Guid lockId,
        TimeSpan duration,
        CancellationToken cancellationToken = default)
    {
        return _repository.LockAsync(
            message.Id,
            lockId,
            duration,
            cancellationToken);
    }

    public Task MarkAsFailedAsync(
        OutboxMessage message,
        string error,
        CancellationToken cancellationToken = default)
    {
        return _repository.MarkAsFailedAsync(
            message.Id,
            error,
            cancellationToken);
    }

    public Task MarkAsProcessedAsync(
        OutboxMessage message,
        CancellationToken cancellationToken = default)
    {
        return _repository.MarkAsProcessedAsync(
            message.Id,
            cancellationToken);
    }
}