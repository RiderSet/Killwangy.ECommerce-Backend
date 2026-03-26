using GBastos.Casa_dos_Farelos.FaturamentoService.Application.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Commands.EmitirFatura;

public record MarcarFaturaComoVencidaCommand(
    Guid FaturaId,
    string IdempotencyKey
) : IIdempotentCommand<Unit>
{
    public MarcarFaturaComoVencidaCommand(Guid faturaId)
        : this(
            faturaId,
            $"marcar-fatura-vencida-{faturaId}-{Guid.NewGuid()}")
    {
    }
}