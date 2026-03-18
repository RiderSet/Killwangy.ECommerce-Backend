using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents.Persistence;
using GBastos.Casa_dos_Farelos.ComprasService.Application.Interfaces;
using GBastos.Casa_dos_Farelos.ComprasService.Domain.Aggregates;

namespace GBastos.Casa_dos_Farelos.ComprasService.Application.Comands.CriarCompra;

public class CriarCompraHandler
{
    private readonly ICompraRepository _repository;
    private readonly IUnitOfWork _uow;

    public CriarCompraHandler(
        ICompraRepository repository,
        IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task<Guid> Handle(
        CriarCompraCommand command,
        CancellationToken ct)
    {
        var compra = Compra.CriarCompra(command.ClienteId);

        await _repository.AddAsync(
            compra,
            ct);

        await _uow.CommitAsync(ct);

        return compra.Id;
    }
}