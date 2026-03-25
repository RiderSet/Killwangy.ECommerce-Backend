using MediatR;

namespace GBastos.Casa_dos_Farelos.CadastroService.Api.Endpoints;

public static class ClienteEndpoints
{
    public static void MapClienteEndpoints(this WebApplication app)
    {
        // Obter cliente por Id
        app.MapGet("/clientes/{id:guid}", async (Guid id, IMediator mediator) =>
            await mediator.Send(new ObterClienteQuery(id)));

        // Listar todos os clientes
        app.MapGet("/clientes", async (IMediator mediator) =>
            await mediator.Send(new ListarClientesQuery()));

        // Criar cliente
        app.MapPost("/clientes", async (CriarClienteCommand command, IMediator mediator) =>
        {
            await mediator.Send(command);
            return Results.Created($"/clientes/{command.Id}", command);
        });

        // Atualizar cliente
        app.MapPut("/clientes/{id:guid}", async (Guid id, AtualizarClienteCommand command, IMediator mediator) =>
        {
            await mediator.Send(command with { Id = id });
            return Results.NoContent();
        });

        // Remover cliente
        app.MapDelete("/clientes/{id:guid}", async (Guid id, IMediator mediator) =>
        {
            await mediator.Send(new RemoverClienteCommand(id));
            return Results.NoContent();
        });
    }
}