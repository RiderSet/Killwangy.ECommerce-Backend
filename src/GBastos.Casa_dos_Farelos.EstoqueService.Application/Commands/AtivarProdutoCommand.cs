using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Common;
using MediatR;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Commands;

public record AtivarProdutoCommand(Guid Id) : IRequest<Result<Guid>>;