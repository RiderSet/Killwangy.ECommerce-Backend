using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;
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

    public Guid AggregateId => ProdutoId;

    public string EventType => nameof(ProdutoCriadoEvent);
    }