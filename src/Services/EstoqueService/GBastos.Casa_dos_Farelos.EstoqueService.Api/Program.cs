using GBastos.Casa_dos_Farelos.Domain.Interfaces;
using GBastos.Casa_dos_Farelos.EstoqueService.Api.Endpoints;
using GBastos.Casa_dos_Farelos.Messaging.Abstractions;
using GBastos.Casa_dos_Farelos.Messaging.RabbitMqPublisher;
using GBastos.Casa_dos_Farelos.PedidoService.Application.Handlers;
using GBastos.Casa_dos_Farelos.SharedKernel.IntegrationEvents.Pedidos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IEventBus, RabbitMqEventBus>();

builder.Services.AddScoped<
    IIntegrationEventHandler<PedidoCriadoIntegrationEvent>,
    PedidoCriadoHandler>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var bus = scope.ServiceProvider.GetRequiredService<IEventBus>();

    bus.Subscribe<
        PedidoCriadoIntegrationEvent,
        PedidoCriadoHandler>();
}

app.MapEstoqueHealth();

app.Run();