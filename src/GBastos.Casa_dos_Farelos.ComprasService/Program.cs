using GBastos.Casa_dos_Farelos.ComprasService.Api.Endpoints;
using GBastos.Casa_dos_Farelos.ComprasService.Application.Interfaces;
using GBastos.Casa_dos_Farelos.ComprasService.Infrastructure.Persistence.Context;
using GBastos.Casa_dos_Farelos.ComprasService.Infrastructure.Repository;
using GBastos.Casa_dos_Farelos.ComprasService.Infrastructure.UOW;
using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.AddEndpointsApiExplorer();

services.AddSwaggerGen();

services.AddDbContext<ComprasDbContext>(options =>
{
    options.UseSqlServer(
        configuration.GetConnectionString("DefaultConnection"));
});

services.AddScoped<ICompraRepository, CompraRepository>();
services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapCompraEndpoints();

app.Run();