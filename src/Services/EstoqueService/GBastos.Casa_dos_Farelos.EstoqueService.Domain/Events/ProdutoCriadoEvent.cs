using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;
using MediatR;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Domain.Events;

public sealed record ProdutoCriadoEvent(
    Guid ProdutoId,
    string Nome,
    decimal Preco
) : IDomainEvent, INotification
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;

    public Guid EventId => Guid.NewGuid();
}