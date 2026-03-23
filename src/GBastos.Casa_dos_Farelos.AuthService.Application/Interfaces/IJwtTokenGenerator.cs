namespace GBastos.Casa_dos_Farelos.AuthService.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string Generate(
        string username,
        string tenantId,
        string role
    );
}