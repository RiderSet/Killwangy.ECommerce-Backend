using CadastroProduto = GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates.Produto;
using EstoqueProduto = GBastos.Casa_dos_Farelos.EstoqueService.Domain.Aggregates.Produto;

namespace GBastos.Casa_dos_Farelos.BuildingBlocks.Mapping;

public static class ProdutoMapper
{
    public static EstoqueProduto ToEstoqueProduto(CadastroProduto produto)
    {
        return new EstoqueProduto(
            produto.Nome,
            produto.DescricaoProduto,
            produto.PrecoVenda,
            produto.PrecoCompra,
            produto.CategoriaId,
            produto.QuantEstoque
        );
    }
}