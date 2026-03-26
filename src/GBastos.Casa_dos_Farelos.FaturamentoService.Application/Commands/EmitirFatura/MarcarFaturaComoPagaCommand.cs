using GBastos.Casa_dos_Farelos.FaturamentoService.Application.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Commands.EmitirFatura;

public record MarcarFaturaComoPagaCommand(
    Guid FaturaId,
    DateTime DataPagamento,
    string IdempotencyKey
) : IIdempotentCommand<Unit>
{
    public MarcarFaturaComoPagaCommand(Guid faturaId)
        : this(
            faturaId,
            DateTime.UtcNow,
            $"marcar-fatura-paga-{faturaId}-{Guid.NewGuid()}")
    {
    }
}