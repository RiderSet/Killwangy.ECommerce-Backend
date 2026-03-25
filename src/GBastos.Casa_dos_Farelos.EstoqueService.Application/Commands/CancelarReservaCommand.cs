using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Common;
using GBastos.Casa_dos_Farelos.EstoqueService.Domain.Entities;
using MediatR;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Commands;

public record CancelarReservaCommand(Guid ReservaId) : IRequest<Result<Reserva>>;