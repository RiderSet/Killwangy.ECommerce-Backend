using GBastos.Casa_dos_Farelos.FaturamentoService.Application.Commands.EmitirFatura;
using GBastos.Casa_dos_Farelos.FaturamentoService.Infrastructure.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Handlers;

public class MarcarFaturaComoPagaHandler : IRequestHandler<MarcarFaturaComoPagaCommand, Unit>
{
    private readonly IFaturaRepository _repository;

    public MarcarFaturaComoPagaHandler(IFaturaRepository repository)
        => _repository = repository;

    public async Task<Unit> Handle(MarcarFaturaComoPagaCommand request, CancellationToken cancellationToken)
    {
        var fatura = await _repository.GetByIdAsync(request.FaturaId, cancellationToken);
        if (fatura is null)
            throw new KeyNotFoundException($"Fatura com Id '{request.FaturaId}' não encontrada.");

        fatura.MarcarComoPaga(); // mantém regras de negócio no Aggregate
        await _repository.UpdateAsync(fatura, cancellationToken);

        return Unit.Value;
    }
}