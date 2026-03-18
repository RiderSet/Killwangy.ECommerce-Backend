using MediatR;

namespace GBastos.Casa_dos_Farelos.ComprasService.Application.Comands.CriarCompra;

public sealed record CriarCompraCommand(
    Guid ClienteId
) : IRequest<Guid>;