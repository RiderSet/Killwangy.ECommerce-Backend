using GBastos.Casa_dos_Farelos.AuthService.Application.Interfaces;

namespace GBastos.Casa_dos_Farelos.AuthService.Infrastructure.Security;

public sealed class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}