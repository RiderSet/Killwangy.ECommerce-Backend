using GBastos.Casa_dos_Farelos.AuthService.Application.Interfaces;
using GBastos.Casa_dos_Farelos.AuthService.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _config;
    private readonly KeyRotationService _keys;

    public JwtTokenGenerator(
        IConfiguration config,
        KeyRotationService keys)
    {
        _config = config;
        _keys = keys;
    }

    public string Generate(
        string username,
        string tenantId,
        string role)
    {
        var jwt = _config.GetSection("Jwt");
        var key = _keys.GetCurrentKey();

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role),
            new Claim("tenant_id", tenantId)
        };

        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}