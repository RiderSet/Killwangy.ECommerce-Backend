using MediatR;

namespace GBastos.Casa_dos_Farelos.CatalogoService.Domain.Events;

public sealed record ProdutoPrecoAtualizadoDomainEvent(
    Guid ProdutoId,
    decimal NovoPreco
) : INotification;