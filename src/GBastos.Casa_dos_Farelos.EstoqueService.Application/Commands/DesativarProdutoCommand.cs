using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Common;
using GBastos.Casa_dos_Farelos.EstoqueService.Domain.Aggregates;
using MediatR;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Commands;

public record DesativarProdutoCommand(Guid Id) : IRequest<Result<Produto>>;