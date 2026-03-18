using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Events;

public sealed record CompraCriadaDomainEvent(
    Guid CompraId,
    Guid ClienteId
) : DomainEvent(CompraId);
