using MediatR;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Commands.EmitirFatura;

public record EmitirFaturaCommand(
    Guid FaturaId,
    string Numero,
    string IdempotencyKey
) : IRequest<Unit>;