using GBastos.Casa_dos_Farelos.FaturamentoService.Application.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Commands.EmitirFatura;

public record CancelarFaturaCommand(
    Guid FaturaId,
    string IdempotencyKey
) : IIdempotentCommand<Unit>
{
    public CancelarFaturaCommand(Guid faturaId)
        : this(faturaId, $"cancelar-fatura-{faturaId}-{Guid.NewGuid()}")
    {
    }
}