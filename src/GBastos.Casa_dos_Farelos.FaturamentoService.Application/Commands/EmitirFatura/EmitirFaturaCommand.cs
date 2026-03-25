using GBastos.Casa_dos_Farelos.FaturamentoService.Application.Interfaces;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Commands.EmitirFatura;

public record EmitirFaturaCommand(
    string Numero,
    string IdempotencyKey
) : IIdempotentCommand<Guid>;