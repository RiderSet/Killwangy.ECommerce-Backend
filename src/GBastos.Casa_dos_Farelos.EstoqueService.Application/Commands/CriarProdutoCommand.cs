using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Common;
using MediatR;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Commands;

public record CriarProdutoCommand(
    string Nome,
    string Descricao,
    decimal PrecoVenda,
    decimal PrecoCompra,
    Guid CategoriaId,
    int QuantEstoque
    ) : IRequest<Result<Guid>>;
