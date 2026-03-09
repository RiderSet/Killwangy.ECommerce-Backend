using GBastos.Casa_dos_Farelos.SharedKernel.DomainEvents;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Domain.Events;

public sealed record EstoqueBaixadoEvent(
    Guid ProdutoId,
    string NomeProduto,
    int Quantidade
) : DomainEvent
{
    public override Guid AggregateId => ProdutoId;
}