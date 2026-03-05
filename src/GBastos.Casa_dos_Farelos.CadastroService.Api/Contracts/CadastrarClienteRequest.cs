namespace GBastos.Casa_dos_Farelos.CadastroService.Api.Contracts;

public sealed class CadastrarClienteRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}