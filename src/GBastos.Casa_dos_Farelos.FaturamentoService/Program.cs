using GBastos.Casa_dos_Farelos.BuildingBlocks.Messaging.RabbitMQ;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;
using GBastos.Casa_dos_Farelos.FaturamentoService.Application.Handlers;
using GBastos.Casa_dos_Farelos.FaturamentoService.Infrastructure.Messaging.Consumer;
using GBastos.Casa_dos_Farelos.FaturamentoService.Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using RabbitMQ.Client;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddLogging();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("Default", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin();
    });
});

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 100;
        opt.Window = TimeSpan.FromSeconds(10);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 10;
    });
});

builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration = configuration.GetConnectionString("Redis");
});

builder.Services.AddDbContext<FaturamentoDbContext>(opt =>
{
    opt.UseSqlServer(
        configuration.GetConnectionString("Default"));
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(PagamentoConfirmadoHandler).Assembly);
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        var jwtKey = configuration["Jwt:Key"] ?? "SUPER_SECRET_KEY";

        opt.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],

            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
});

builder.Services.AddHostedService<FaturamentoConsumer>();
builder.Services.AddHealthChecks();

#region RabbitMQ

builder.Services.AddSingleton<IConnection>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();

    var factory = new ConnectionFactory
    {
        HostName = configuration["RabbitMQ:Host"] ?? "localhost",
        UserName = configuration["RabbitMQ:User"] ?? "guest",
        Password = configuration["RabbitMQ:Password"] ?? "guest"
    };

    return factory.CreateConnectionAsync()
                  .GetAwaiter()
                  .GetResult();
});

builder.Services.AddSingleton<IEventPublisher, RabbitMqPublisher>();

#endregion

var app = builder.Build();

app.UseCors("Default");

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

builder.Services.AddSingleton<IChannel>(sp =>
{
    var connection = sp.GetRequiredService<IConnection>();

    return connection.CreateChannelAsync()
                     .GetAwaiter()
                     .GetResult();
});

app.MapHealthChecks("/health");

var group = app.MapGroup("/api/v1/faturas")
    .RequireAuthorization()
    .RequireRateLimiting("fixed")
    .WithTags("Faturas");

app.Run();