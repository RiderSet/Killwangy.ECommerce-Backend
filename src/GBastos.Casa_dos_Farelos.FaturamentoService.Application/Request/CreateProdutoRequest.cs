namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Request;

public record CreateProdutoRequest(
    string Nome,
    string Descricao,
    decimal Preco,
    Guid CategoriaId,
    int QuantEstoque
);