using DotNetEnv;
using GBastos.Casa_dos_Farelos.CadastroService.Api.Endpoints;
using GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Persistence.Context;
using GBastos.Casa_dos_Farelos.SharedKernel.DomainEvents;
using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Serilog;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

#region ===== Logging Enterprise =====

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

#endregion

#region ===== Configuration =====

var configuration = builder.Configuration;

#endregion

#region ===== Database MySQL =====

builder.Services.AddDbContext<CadastroDbContext>(options =>
{
    var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
    var user = Environment.GetEnvironmentVariable("DB_USER");
    var database = Environment.GetEnvironmentVariable("DB_DATABASE");
    var server = Environment.GetEnvironmentVariable("DB_SERVER");
    var port = Environment.GetEnvironmentVariable("DB_PORT");

    var connectionString =
        $"server={server};port={port};database={database};user={user};password={password}";

    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString),
        mysql =>
        {
            mysql.EnableRetryOnFailure(3);
        });
});

#endregion

#region ===== DI Services =====

builder.Services.AddScoped<IEventBus, InMemoryEventBus>();

builder.Services.AddControllers();

#endregion

#region ===== Swagger =====

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Casa dos Farelos - CadastroService",
        Version = "v1",
        Description = "API Enterprise - Cadastro Service"
    });
});

#endregion

#region ===== HealthChecks =====

builder.Services.AddHealthChecks()
    .AddDbContextCheck<CadastroDbContext>("cadastro-db");

#endregion

#region ===== CORS =====

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

#endregion

var app = builder.Build();

#region ===== Middleware Pipeline =====

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCadastroEndpoints();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseSerilogRequestLogging();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

#endregion

app.Run();