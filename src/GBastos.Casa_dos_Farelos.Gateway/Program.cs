using GBastos.Casa_dos_Farelos.Gateway.Extensions;
using GBastos.Casa_dos_Farelos.Gateway.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// builder.Configuration.AddKeyVaultSecrets(builder.Configuration);

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/gateway-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

//builder.Configuration.AddCloudConfiguration(
//    builder.Configuration,
//    builder.Environment);

builder.Host.UseSerilog();

builder.Services.AddGatewayReverseProxy(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddGatewayRateLimiting();
builder.Services.AddGatewayObservability();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseMiddleware<TenantResolverMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();
app.UseGatewayReverseProxy();

app.Run();