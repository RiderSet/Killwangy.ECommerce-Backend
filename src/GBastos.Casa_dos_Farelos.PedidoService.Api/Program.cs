using GBastos.Casa_dos_Farelos.Messaging.RabbitMqPublisher;
using GBastos.Casa_dos_Farelos.PedidoService.Api.Endpoints;
using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IEventBus, RabbitMqEventBus>();

var app = builder.Build();

app.MapPedidoEndpoints();

app.Run();