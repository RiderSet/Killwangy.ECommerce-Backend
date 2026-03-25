namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.DTOs;

public record FaturaDto(
    Guid Id,
    string Numero,
    DateTime DataEmissao,
    DateTime DataVencimento,
    string Status,
    decimal ValorTotal,
    IReadOnlyCollection<ItemFaturaDto> Itens
);