using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;
using GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Messaging.EventBus;
using GBastos.Casa_dos_Farelos.PedidoService.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IEventBus, RabbitMqEventBus>();

var app = builder.Build();

app.MapPedidoEndpoints();

app.Run();