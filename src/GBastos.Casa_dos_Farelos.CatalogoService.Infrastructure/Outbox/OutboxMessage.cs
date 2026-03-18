using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Abstractions;

namespace GBastos.Casa_dos_Farelos.CatalogoService.Infrastructure.Outbox;

public sealed class OutboxMessage : BaseEntity
{
    public DateTime OccurredOnUtc { get; private set; }
    public string Type { get; private set; } = default!;
    public string Payload { get; private set; } = default!;
    public DateTime? ProcessedOnUtc { get; private set; }
    public string? Error { get; private set; }

    private OutboxMessage() { }

    public OutboxMessage(
        DateTime occurredOnUtc,
        string type,
        string content)
    {
        OccurredOnUtc = occurredOnUtc;
        Type = type;
        Payload = content;
    }

    public void MarkAsProcessed()
    {
        ProcessedOnUtc = DateTime.UtcNow;
        Error = null;
    }

    public void MarkAsFailed(string error)
    {
        ProcessedOnUtc = DateTime.UtcNow;
        Error = error;
    }
}