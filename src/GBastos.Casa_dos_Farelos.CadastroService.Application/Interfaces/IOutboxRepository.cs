using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.CadastroService.Application.Interfaces;

public interface IOutboxRepository
{
    Task AddAsync(IDomainEvent domainEvent, CancellationToken ct);

    Task AddAsync<T>(T integrationEvent, CancellationToken ct)
        where T : IIntegrationEvent;

    Task SaveChangesAsync(CancellationToken ct);
}