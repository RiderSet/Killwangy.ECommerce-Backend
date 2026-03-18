using GBastos.Casa_dos_Farelos.EstoqueService.Api.Endpoints.V1;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.OpenApi;
using System.Reflection;
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

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

    // cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    // cfg.AddOpenBehavior(typeof(IdempotencyBehavior<,>));
    // cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

var jwtKey = configuration["Jwt:Key"] ?? "SUPER_SECRET_KEY_CHANGE_ME";

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
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

builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseCors("Default");
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler(error =>
{
    error.Run(async context =>
    {
        var exception = context.Features
            .Get<IExceptionHandlerFeature>()?
            .Error;

        var problem = new ProblemDetails
        {
            Title = "Internal Server Error",
            Status = 500,
            Detail = exception?.Message
        };

        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(problem);
    });
});

app.MapHealthChecks("/health");

app.MapProdutoEndpointsV1()
   .RequireRateLimiting("fixed")
   .RequireAuthorization();

app.Run();