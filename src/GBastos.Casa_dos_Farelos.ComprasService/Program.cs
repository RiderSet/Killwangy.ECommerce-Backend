using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Events;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents.Persistence;
using GBastos.Casa_dos_Farelos.ComprasService.Application.Comands.CriarCompra;
using GBastos.Casa_dos_Farelos.ComprasService.Application.Interfaces;
using GBastos.Casa_dos_Farelos.ComprasService.Infrastructure.Interfaces;
using GBastos.Casa_dos_Farelos.ComprasService.Infrastructure.Outbox;
using GBastos.Casa_dos_Farelos.ComprasService.Infrastructure.Persistence.Context;
using GBastos.Casa_dos_Farelos.ComprasService.Infrastructure.Repository;
using GBastos.Casa_dos_Farelos.ComprasService.Infrastructure.UOW;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ComprasDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ComprasConnection")));

builder.Services.AddScoped<ICompraRepository, CompraRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddMediatR(typeof(CriarCompraHandler).Assembly);

builder.Services.AddScoped<IOutboxRepository, OutboxRepository>();
builder.Services.AddScoped<IEventBus, InMemoryEventBus>();
builder.Services.AddHostedService<OutboxProcessor>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();