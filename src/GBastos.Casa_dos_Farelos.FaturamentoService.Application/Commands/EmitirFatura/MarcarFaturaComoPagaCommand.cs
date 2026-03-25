using GBastos.Casa_dos_Farelos.FaturamentoService.Application.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Commands.EmitirFatura;

public record MarcarFaturaComoPagaCommand(
    Guid FaturaId,
    DateTime DataPagamento,
    string IdempotencyKey
) : IIdempotentCommand<Unit>;