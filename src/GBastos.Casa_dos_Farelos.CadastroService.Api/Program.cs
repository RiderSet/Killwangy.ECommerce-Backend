using CorrelationId;
using CorrelationId.DependencyInjection;
using DotNetEnv;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.DomainEvents;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;
using GBastos.Casa_dos_Farelos.CadastroService.Api.Endpoints;
using GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using StackExchange.Redis;


Env.Load();

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

var configuration = builder.Configuration;

builder.Services.AddDefaultCorrelationId(options =>
{
    options.RequestHeader = "X-Correlation-ID";
    options.IncludeInResponse = true;
});

var redisConnection =
    configuration["Redis:ConnectionString"]
    ?? throw new InvalidOperationException("Redis connection string missing");

builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(redisConnection));

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

// Tracing
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource =>
    {
        resource.AddService(
            serviceName: "CadastroService",
            serviceVersion: "1.0.0");
    })
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddConsoleExporter();
    })
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation();
    });

builder.Services.AddHealthChecks()
    .AddDbContextCheck<CadastroDbContext>("cadastro-db")
    .AddRedis(redisConnection, name: "redis");

builder.Services.AddScoped<IEventBus, InMemoryEventBus>();

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("api", config =>
    {
        config.Window = TimeSpan.FromSeconds(10);
        config.PermitLimit = 100;
        config.QueueLimit = 2;
    });
});

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = ctx =>
    {
        ctx.ProblemDetails.Extensions["traceId"] =
            ctx.HttpContext.TraceIdentifier;
    };
});

builder.Services.AddControllers();
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

builder.Services.AddHealthChecks()
    .AddDbContextCheck<CadastroDbContext>("cadastro-db")
    .AddRedis(redisConnection, "redis");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCorrelationId();

app.UseSerilogRequestLogging();
app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseRateLimiter();
app.UseAuthorization();
app.MapCadastroEndpoints();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();