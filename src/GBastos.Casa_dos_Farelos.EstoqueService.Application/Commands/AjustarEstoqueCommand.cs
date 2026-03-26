using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Common;
using GBastos.Casa_dos_Farelos.EstoqueService.Application.Enums;
using MediatR;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Commands;

public record AjustarEstoqueCommand(
    Guid ProdutoId,
    int Quantidade,
    TipoAjusteEstoque Tipo
) : IRequest<Result<Guid>>;