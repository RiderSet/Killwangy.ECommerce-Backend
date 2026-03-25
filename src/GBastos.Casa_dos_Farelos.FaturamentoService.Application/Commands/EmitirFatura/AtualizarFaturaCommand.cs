using GBastos.Casa_dos_Farelos.FaturamentoService.Application.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Commands.EmitirFatura;

public record AtualizarFaturaCommand(
    Guid Id,
    DateTime DataVencimento,
    decimal ValorTotal,
    string IdempotencyKey
) : IIdempotentCommand<Unit>;