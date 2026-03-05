namespace GBastos.Casa_dos_Farelos.CadastroService.Api.Contracts;

public record CriarClienteRequest(
    string Nome,
    string Email,
    string Documento);