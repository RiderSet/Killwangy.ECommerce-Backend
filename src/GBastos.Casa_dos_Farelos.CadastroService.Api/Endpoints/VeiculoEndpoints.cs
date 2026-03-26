using GBastos.Casa_dos_Farelos.CadastroService.Application.Commands.Veiculos;
using GBastos.Casa_dos_Farelos.CadastroService.Application.Queries.Veiculos.ListarVeiculo;
using GBastos.Casa_dos_Farelos.CadastroService.Application.Queries.Veiculos.ObterVeiculo;
using MediatR;

namespace GBastos.Casa_dos_Farelos.CadastroService.Api.Endpoints;

public static class VeiculoEndpoints
{
    public static void MapVeiculoEndpoints(this WebApplication app)
    {
        // Obter veículo por placa
        app.MapGet("/veiculos/{placa}", async (string placa, IMediator mediator) =>
            await mediator.Send(new ObterVeiculoQuery(placa)));

        // Listar todos os veículos
        app.MapGet("/veiculos", async (IMediator mediator) =>
            await mediator.Send(new ListarVeiculosQuery()));

        // Criar veículo
        app.MapPost("/veiculos", async (CriarVeiculoCommand command, IMediator mediator) =>
        {
            await mediator.Send(command);
            return Results.Created($"/veiculos/{command.Placa}", command);
        });

        // Atualizar veículo
        app.MapPut("/veiculos/{id:guid}", async (Guid id, AtualizarVeiculoCommand command, IMediator mediator) =>
        {
            await mediator.Send(command with { Id = id });
            return Results.NoContent();
        });

        // Remover veículo
        app.MapDelete("/veiculos/{id:guid}", async (Guid id, IMediator mediator) =>
        {
            await mediator.Send(new RemoverVeiculoCommand(id));
            return Results.NoContent();
        });
    }
}