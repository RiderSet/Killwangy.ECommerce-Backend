using GBastos.Casa_dos_Farelos.AuthService.Application.Configurations;
using GBastos.Casa_dos_Farelos.AuthService.Application.Interfaces;
using GBastos.Casa_dos_Farelos.AuthService.Application.Services;
using GBastos.Casa_dos_Farelos.AuthService.Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("Jwt"));

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<TokenValidator>();

builder.Services.AddSingleton<KeyRotationService>();
builder.Services.AddHostedService<KeyRotationHostedService>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapAuthEndpoints();

app.Run();