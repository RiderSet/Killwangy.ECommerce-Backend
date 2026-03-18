namespace GBastos.Casa_dos_Farelos.CadastroService.Application.DTOs;

public sealed class ClienteDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Documento { get; set; } = default!;
}