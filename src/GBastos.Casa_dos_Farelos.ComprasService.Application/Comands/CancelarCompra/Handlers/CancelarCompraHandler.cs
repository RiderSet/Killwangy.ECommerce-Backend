using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents.Persistence;
using GBastos.Casa_dos_Farelos.ComprasService.Application.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.ComprasService.Application.Comands.CancelarCompra.Handlers;

internal sealed class CancelarCompraHandler
    : IRequestHandler<CancelarCompraCommand, Unit>
{
    private readonly ICompraRepository _repository;
    private readonly IUnitOfWork _uow;

    public CancelarCompraHandler(
        ICompraRepository repository,
        IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task<Unit> Handle(
        CancelarCompraCommand request,
        CancellationToken ct)
    {
        var compra = await _repository.GetByIdAsync(request.CompraId, ct);

        if (compra is null)
            throw new InvalidOperationException("Compra não encontrada.");

        compra.Cancelar();
        _repository.Update(compra);

        await _uow.CommitAsync(ct);
        return Unit.Value;
    }
}