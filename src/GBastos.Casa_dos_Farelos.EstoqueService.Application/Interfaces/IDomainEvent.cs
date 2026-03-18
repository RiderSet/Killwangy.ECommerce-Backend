namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;

public interface IDomainEvent
{
    Guid Id { get; }
    DateTime OccurredOnUtc { get; }
}