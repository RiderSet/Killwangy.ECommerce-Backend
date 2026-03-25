using GBastos.Casa_dos_Farelos.FaturamentoService.Application.Commands.EmitirFatura;
using GBastos.Casa_dos_Farelos.FaturamentoService.Infrastructure.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Handlers;

public class AdicionarItemFaturaHandler : IRequestHandler<AdicionarItemFaturaCommand, Unit>
{
    private readonly IFaturaRepository _repository;

    public AdicionarItemFaturaHandler(IFaturaRepository repository)
        => _repository = repository;

    public async Task<Unit> Handle(AdicionarItemFaturaCommand request, CancellationToken cancellationToken)
    {
        var fatura = await _repository.GetByIdAsync(request.FaturaId, cancellationToken);
        if (fatura is null)
            throw new KeyNotFoundException($"Fatura {request.FaturaId} não encontrada.");

        // Adiciona item usando o Aggregate
        fatura.AdicionarItem(request.ProdutoId, request.Quantidade, request.ValorUnitario);

        await _repository.UpdateAsync(fatura, cancellationToken);
        return Unit.Value;
    }
}