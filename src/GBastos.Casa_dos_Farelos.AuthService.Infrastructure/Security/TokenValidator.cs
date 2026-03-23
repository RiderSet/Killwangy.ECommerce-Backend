using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GBastos.Casa_dos_Farelos.AuthService.Infrastructure.Security;

public class TokenValidator
{
    private readonly IConfiguration _configuration;
    private readonly KeyRotationService _keyRotation;

    public TokenValidator(
        IConfiguration configuration,
        KeyRotationService keyRotation)
    {
        _configuration = configuration;
        _keyRotation = keyRotation;
    }

    public ClaimsPrincipal? Validate(string token)
    {
        var jwt = _configuration.GetSection("Jwt");

        var handler = new JwtSecurityTokenHandler();

        var parameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],

            IssuerSigningKeys = _keyRotation.GetAllKeys(),

            ClockSkew = TimeSpan.FromMinutes(1)
        };

        try
        {
            var principal = handler.ValidateToken(
                token,
                parameters,
                out var validatedToken);

            return principal;
        }
        catch
        {
            return null;
        }
    }

    public string? GetTenantId(ClaimsPrincipal principal)
    {
        return principal
            .Claims
            .FirstOrDefault(x => x.Type == "tenant_id")
            ?.Value;
    }

    public string? GetUsername(ClaimsPrincipal principal)
    {
        return principal.Identity?.Name;
    }

    public string? GetRole(ClaimsPrincipal principal)
    {
        return principal
            .Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.Role)
            ?.Value;
    }
}