using GBastos.Casa_dos_Farelos.EstoqueService.Application.DTOs;
using GBastos.Casa_dos_Farelos.EstoqueService.Domain.Events;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;

public interface IOutboxRepositoryES
{
    Task AddAsync(EstoqueConfirmadoEvent evento, CancellationToken cancellationToken);
    Task<List<OutboxMessageDto>> GetUnprocessedAsync(
        int take,
        CancellationToken ct);
}