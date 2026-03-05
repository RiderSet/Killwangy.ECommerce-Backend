namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.DTOs;

public sealed class OutboxMessageDto
{
    public Guid Id { get; init; }
    public string Type { get; init; } = null!;
    public string Payload { get; init; } = null!;
    public DateTime OccurredOnUtc { get; init; }
}