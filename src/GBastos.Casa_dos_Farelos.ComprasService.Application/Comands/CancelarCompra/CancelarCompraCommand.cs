using MediatR;

namespace GBastos.Casa_dos_Farelos.ComprasService.Application.Comands.CancelarCompra;

public sealed record CancelarCompraCommand(Guid CompraId) : IRequest<Unit>;