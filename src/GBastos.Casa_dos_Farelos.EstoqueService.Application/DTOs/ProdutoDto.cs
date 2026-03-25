namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.DTOs;

public record ProdutoDto(
    Guid Id,
    string Nome,
    string Descricao,
    decimal PrecoVenda,
    decimal PrecoCompra,
    Guid CategoriaId,
    int QuantEstoque,
    bool Ativo
);