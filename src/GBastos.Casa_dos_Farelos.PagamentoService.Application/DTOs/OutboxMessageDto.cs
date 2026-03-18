namespace GBastos.Casa_dos_Farelos.PagamentoService.Application.DTOs;

public sealed class OutboxMessageDto
{
    public Guid Id { get; set; }
    public string Type { get; set; } = null!;
    public string Payload { get; set; } = null!;
    public DateTime OccurredOnUtc { get; set; }
}