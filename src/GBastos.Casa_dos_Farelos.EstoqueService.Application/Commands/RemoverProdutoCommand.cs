using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Primitives;
using MediatR;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Commands;

public record RemoverProdutoCommand(Guid Id) : IRequest<Result>;