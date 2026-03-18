using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.IntegrationEvents;

namespace GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.IntegrationEvents.Cadastros;

public sealed class ClienteCadastradoIntegrationEvent : IIntegrationEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
    public string EventType => nameof(ClienteCadastradoIntegrationEvent);
    public int Version => 1;

    public Guid ClienteId { get; init; }
    public string? Nome { get; init; }
    public string? Email { get; init; }
}