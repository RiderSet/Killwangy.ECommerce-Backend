using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.ComprasService.Domain.Events;

public sealed record ProdutoCriadoEvent(
    Guid ProdutoId,
    string Nome,
    decimal PrecoVenda
) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}