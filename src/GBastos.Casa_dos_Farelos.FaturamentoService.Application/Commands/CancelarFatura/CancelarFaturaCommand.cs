using GBastos.Casa_dos_Farelos.FaturamentoService.Application.Interfaces;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Commands.CancelarFatura;

public record CancelarFaturaCommand(
    Guid Id,
    string IdempotencyKey
) : IIdempotentCommand<Guid>;
