using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Events;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Domain.Events;

public sealed record ReservaCanceladaDomainEvent(
    Guid ProdutoId,
    Guid ReservaId
) : DomainEvent(ProdutoId);