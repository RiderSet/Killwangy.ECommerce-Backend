namespace GBastos.Casa_dos_Farelos.CadastroService.Application.Request;

public record CreateProdutoRequest(
    string Nome,
    string Descricao
);