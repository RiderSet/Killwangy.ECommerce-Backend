using GBastos.Casa_dos_Farelos.AuthService.Api.Endpoints;
using GBastos.Casa_dos_Farelos.AuthService.Application.Configurations;
using GBastos.Casa_dos_Farelos.AuthService.Application.Interfaces;
using GBastos.Casa_dos_Farelos.AuthService.Application.Services;
using GBastos.Casa_dos_Farelos.AuthService.Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("Jwt"));

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddHostedService<KeyRotationHostedService>();
builder.Services.AddSingleton<KeyRotationService>();
builder.Services.AddScoped<TokenValidator>();

builder.Services.AddScoped<JwtTokenService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapAuthEndpoints();

app.Run();