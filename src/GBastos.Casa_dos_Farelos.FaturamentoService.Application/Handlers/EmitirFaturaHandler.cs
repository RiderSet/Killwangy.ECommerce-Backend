using GBastos.Casa_dos_Farelos.FaturamentoService.Application.Commands.EmitirFatura;
using MediatR;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Handlers;

public class EmitirFaturaHandler
    : IRequestHandler<EmitirFaturaCommand, Guid>
{
    public Task<Guid> Handle(
        EmitirFaturaCommand request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(Guid.NewGuid());
    }
}