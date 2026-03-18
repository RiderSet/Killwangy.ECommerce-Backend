using GBastos.Casa_dos_Farelos.AuthService.Api.Endpoints;
using GBastos.Casa_dos_Farelos.AuthService.Application.Configurations;
using GBastos.Casa_dos_Farelos.AuthService.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("Jwt"));

builder.Services.AddScoped<JwtTokenService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapAuthEndpoints();

app.Run();