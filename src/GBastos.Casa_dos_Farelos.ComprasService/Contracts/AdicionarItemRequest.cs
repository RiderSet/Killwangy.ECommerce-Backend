namespace GBastos.Casa_dos_Farelos.ComprasService.Api.Contracts;

public record AdicionarItemRequest(
    Guid ProdutoId,
    string NomeProduto,
    int Quantidade,
    decimal CustoUnitario);