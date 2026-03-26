namespace GBastos.Casa_dos_Farelos.CadastroService.Api.Contracts;

public record AtualizarClienteRequest(
    string Nome,
    string Telefone,
    string Email);