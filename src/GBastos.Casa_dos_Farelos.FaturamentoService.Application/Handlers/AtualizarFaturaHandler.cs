using GBastos.Casa_dos_Farelos.FaturamentoService.Application.Commands.EmitirFatura;
using GBastos.Casa_dos_Farelos.FaturamentoService.Infrastructure.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Handlers;

public class AtualizarFaturaHandler : IRequestHandler<AtualizarFaturaCommand, Unit>
{
    private readonly IFaturaRepository _repository;

    public AtualizarFaturaHandler(IFaturaRepository repository)
        => _repository = repository;

    public async Task<Unit> Handle(AtualizarFaturaCommand request, CancellationToken cancellationToken)
    {
        var fatura = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (fatura is null)
            throw new KeyNotFoundException($"Fatura {request.Id} não encontrada.");

        // Atualiza usando método do Aggregate
        fatura.AtualizarDataVencimento(request.DataVencimento);

        await _repository.UpdateAsync(fatura, cancellationToken);
        return Unit.Value;
    }
}