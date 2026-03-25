
using GBastos.Casa_dos_Farelos.FaturamentoService.Application.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Commands.CancelarFatura;

public record DeletarFaturaCommand(
    Guid Id,
    string IdempotencyKey
) : IIdempotentCommand<Unit>;