namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.DTOs;

public record ItemFaturaDto(
    Guid ProdutoId,
    int Quantidade,
    decimal ValorUnitario,
    decimal Total
);