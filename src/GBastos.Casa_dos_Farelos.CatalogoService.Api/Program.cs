using GBastos.Casa_dos_Farelos.CatalogoService.Api.Endpoints;
using GBastos.Casa_dos_Farelos.CatalogoService.Application.Commands;
using GBastos.Casa_dos_Farelos.CatalogoService.Infrastructure.Interfaces;
using GBastos.Casa_dos_Farelos.CatalogoService.Infrastructure.Persistence;
using GBastos.Casa_dos_Farelos.CatalogoService.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var environment = builder.Environment;

builder.Services.AddDbContext<CatalogoDbContext>(options =>
{
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

builder.Services.AddMediatR(typeof(CriarProdutoCommand).Assembly);
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapProdutoEndpoints();

app.Run();