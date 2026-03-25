using GBastos.Casa_dos_Farelos.FaturamentoService.Application.Commands.EmitirFatura;
using GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Aggregates;
using GBastos.Casa_dos_Farelos.FaturamentoService.Infrastructure.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Handlers;

public class CriarFaturaHandler : IRequestHandler<CriarFaturaCommand, Guid>
{
    private readonly IFaturaRepository _repository;

    public CriarFaturaHandler(IFaturaRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CriarFaturaCommand request, CancellationToken cancellationToken)
    {
        var fatura = new Fatura(request.Numero, request.ClienteId, request.PedidoId, request.DataVencimento);
        fatura.Emitir();

        await _repository.AddAsync(fatura, cancellationToken);

        return fatura.Id;
    }
}