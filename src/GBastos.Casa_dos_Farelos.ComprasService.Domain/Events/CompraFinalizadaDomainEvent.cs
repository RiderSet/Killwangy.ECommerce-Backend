using MediatR;

namespace GBastos.Casa_dos_Farelos.ComprasService.Domain.Events;

public sealed record CompraFinalizadaDomainEvent(
    Guid CompraId,
    Guid ProdutoId,
    string NomeProduto,
    int Quantidade
) : INotification;