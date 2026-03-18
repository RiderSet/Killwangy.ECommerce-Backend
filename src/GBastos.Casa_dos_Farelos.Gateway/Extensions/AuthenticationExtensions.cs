using GBastos.Casa_dos_Farelos.Gateway.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GBastos.Casa_dos_Farelos.Gateway.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwt = configuration
            .GetSection("Jwt")
            .Get<JwtOptions>()!;

        services.AddAuthentication(
            JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = true;
            options.SaveToken = true;

            options.TokenValidationParameters =
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,

                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwt.Key)),

                    ClockSkew = TimeSpan.FromMinutes(1)
                };

            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    var tenantId =
                        context.Principal?.Claims
                        .FirstOrDefault(x =>
                            x.Type == "tenant_id")?.Value;

                    if (string.IsNullOrEmpty(tenantId))
                        context.Fail("Tenant inválido");

                    return Task.CompletedTask;
                }
            };
        });

        services.AddAuthorization();

        return services;
    }
}