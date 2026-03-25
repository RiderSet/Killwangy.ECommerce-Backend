using GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Interfaces;
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
        CancellationToken ct)
    {
        var veiculo = await _repository.GetByPlacaAsync(
            request.Placa,
            ct);

        if (veiculo is null)
                veiculo = await _repository.GetByIdAsync(
                request.Id,
                ct);

        if (veiculo is null)
            throw new InvalidOperationException("Veículo não encontrado.");

        await _repository.RemoveAsync(veiculo, ct);

        return Unit.Value;
    }
}