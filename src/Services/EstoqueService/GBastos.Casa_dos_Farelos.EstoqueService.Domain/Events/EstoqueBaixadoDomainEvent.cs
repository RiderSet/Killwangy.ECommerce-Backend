using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;
using MediatR;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Domain.Events;

public sealed record EstoqueBaixadoDomainEvent(
    Guid ProdutoId,
    string NomeProduto,
    int Quantidade
) : IDomainEvent, INotification
{
    public Guid EventId => Guid.NewGuid();
    public DateTime OccurredOnUtc => DateTime.UtcNow;
}
