using MediatR;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Commands.EmitirFatura;

public record CancelarFaturaCommand(
    Guid FaturaId,
    string IdempotencyKey
) : IRequest<Unit>;