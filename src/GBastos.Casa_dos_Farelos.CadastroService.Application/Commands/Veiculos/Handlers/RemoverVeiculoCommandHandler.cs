using GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates;
using MediatR;

namespace GBastos.Casa_dos_Farelos.CadastroService.Application.Commands.Veiculos.Handlers;

public sealed class RemoverVeiculoCommandHandler
    : IRequestHandler<RemoverVeiculoCommand, Unit>
{
    private readonly IVeiculoRepository _repository;

    public RemoverVeiculoCommandHandler(
        IVeiculoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(
        RemoverVeiculoCommand request,
        CancellationToken cancellationToken)
    {
        var veiculo = await _repository.GetByIdAsync(
            request.VeiculoId,
            cancellationToken);

        if (veiculo is null)
            throw new InvalidOperationException("Veículo não encontrado.");

        await _repository.RemoveAsync(veiculo, cancellationToken);

        return Unit.Value;
    }
}