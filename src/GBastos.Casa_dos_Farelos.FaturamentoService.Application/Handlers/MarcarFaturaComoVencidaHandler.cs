using GBastos.Casa_dos_Farelos.FaturamentoService.Application.Commands.EmitirFatura;
using GBastos.Casa_dos_Farelos.FaturamentoService.Infrastructure.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Handlers;

public class MarcarFaturaComoVencidaHandler : IRequestHandler<MarcarFaturaComoVencidaCommand, Unit>
{
    private readonly IFaturaRepository _repository;

    public MarcarFaturaComoVencidaHandler(IFaturaRepository repository)
        => _repository = repository;

    public async Task<Unit> Handle(MarcarFaturaComoVencidaCommand request, CancellationToken cancellationToken)
    {
        var fatura = await _repository.GetByIdAsync(request.FaturaId, cancellationToken);
        if (fatura is null)
            throw new KeyNotFoundException($"Fatura {request.FaturaId} não encontrada.");

        fatura.MarcarComoVencida();
        await _repository.UpdateAsync(fatura, cancellationToken);

        return Unit.Value;
    }
}