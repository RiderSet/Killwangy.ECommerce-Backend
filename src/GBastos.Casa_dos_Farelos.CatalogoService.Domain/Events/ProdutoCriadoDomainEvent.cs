using MediatR;

namespace GBastos.Casa_dos_Farelos.CatalogoService.Domain.Events;

public record ProdutoCriadoDomainEvent(
    Guid ProdutoId,
    string Nome,
    decimal Preco
) : INotification;