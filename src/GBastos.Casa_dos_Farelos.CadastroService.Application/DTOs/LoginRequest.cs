namespace GBastos.Casa_dos_Farelos.CadastroService.Application.DTOs;

public sealed record LoginRequest(
    string Email,
    string Password
);