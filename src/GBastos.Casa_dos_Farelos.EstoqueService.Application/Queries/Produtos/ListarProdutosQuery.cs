using GBastos.Casa_dos_Farelos.EstoqueService.Application.DTOs;
using MediatR;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Queries.Produtos;

public record ListarProdutosQuery(int Page, int PageSize) : IRequest<IEnumerable<ProdutoDto>>;