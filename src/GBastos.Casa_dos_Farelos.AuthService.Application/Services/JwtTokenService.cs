using GBastos.Casa_dos_Farelos.AuthService.Application.Configurations;
using GBastos.Casa_dos_Farelos.AuthService.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GBastos.Casa_dos_Farelos.AuthService.Application.Services;

public sealed class JwtTokenService
{
    private readonly JwtOptions _jwt;

    public JwtTokenService(JwtOptions jwt)
    {
        _jwt = jwt;
    }

    public string GenerateToken(User user, Tenant tenant)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("tenant_id", tenant.Id.ToString()),
            new Claim(ClaimTypes.Role, "admin")
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwt.Key));

        var creds = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(_jwt.ExpirationHours),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}