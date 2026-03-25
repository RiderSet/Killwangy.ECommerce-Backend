using GBastos.Casa_dos_Farelos.FaturamentoService.Application.Interfaces;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Commands.EmitirFatura;

public record CriarFaturaCommand(
    string Numero,
    Guid ClienteId,
    Guid PedidoId,
    DateTime DataVencimento,
    decimal ValorTotal,
    string IdempotencyKey
) : IIdempotentCommand<Guid>;