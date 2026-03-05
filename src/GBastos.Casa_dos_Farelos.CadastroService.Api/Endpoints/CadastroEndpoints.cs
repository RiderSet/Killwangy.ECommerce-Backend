using GBastos.Casa_dos_Farelos.CadastroService.Application.Commands.Veiculos;
using MassTransit.Mediator;

namespace GBastos.Casa_dos_Farelos.CadastroService.Api.Endpoints;

public static class CadastroEndpoints
{
    public static IEndpointRouteBuilder MapCadastroEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/cadastro");

        // Health
        group.MapGet("/health", Health);

        // Cliente
        group.MapPost("/clientes", CriarClienteAsync);
        group.MapGet("/clientes/{id:guid}", ObterClienteAsync);
        group.MapGet("/clientes", ListarClientesAsync);
        group.MapPut("/clientes/{id:guid}", AtualizarClienteAsync);
        group.MapDelete("/clientes/{id:guid}", RemoverClienteAsync);

        // Veículo
        group.MapPost("/veiculos", CriarVeiculoAsync);
        group.MapGet("/veiculos/{placa}", ObterVeiculoAsync);
        group.MapGet("/veiculos", ListarVeiculosAsync);
        group.MapDelete("/veiculos/{placa}", RemoverVeiculoAsync);

        return endpoints;
    }

    // HEALTH

    private static IResult Health()
    {
        return Results.Ok(new
        {
            Service = "CadastroService",
            Status = "Healthy",
            Timestamp = DateTime.UtcNow
        });
    }

    // CLIENTE

    private static async Task<IResult> CriarClienteAsync(
        CriarClienteRequest request,
        IMediator mediator)
    {
        var command = new CriarClienteCommand(
            request.Nome,
            request.Email,
            request.Documento);

        var id = await mediator.Send(command);

        return Results.Created($"/cadastro/clientes/{id}", new { id });
    }

    private static async Task<IResult> ObterClienteAsync(
        Guid id,
        IMediator mediator)
    {
        var cliente = await mediator.Send(
            new ObterClienteQuery(id));

        return cliente is null
            ? Results.NotFound()
            : Results.Ok(cliente);
    }

    private static async Task<IResult> ListarClientesAsync(
        IMediator mediator)
    {
        var clientes = await mediator.Send(
            new ListarClientesQuery());

        return Results.Ok(clientes);
    }

    private static async Task<IResult> AtualizarClienteAsync(
        Guid id,
        AtualizarClienteRequest request,
        IMediator mediator)
    {
        var command = new AtualizarClienteCommand(
            id,
            request.Nome,
            request.Email);

        await mediator.Send(command);

        return Results.NoContent();
    }

    private static async Task<IResult> RemoverClienteAsync(
        Guid id,
        IMediator mediator)
    {
        await mediator.Send(
            new RemoverClienteCommand(id));

        return Results.NoContent();
    }

    // VEICULO

    private static async Task<IResult> CriarVeiculoAsync(
        CriarVeiculoRequest request,
        IMediator mediator)
    {
        var command = new CriarVeiculoCommand(
            request.Placa,
            request.Modelo,
            request.ClienteId);

        await mediator.Send(command);

        return Results.Ok();
    }

    private static async Task<IResult> ObterVeiculoAsync(
        string placa,
        IMediator mediator)
    {
        var veiculo = await mediator.Send(
            new ObterVeiculoQuery(placa));

        return veiculo is null
            ? Results.NotFound()
            : Results.Ok(veiculo);
    }

    private static async Task<IResult> ListarVeiculosAsync(
        IMediator mediator)
    {
        var veiculos = await mediator.Send(
            new ListarVeiculosQuery());

        return Results.Ok(veiculos);
    }

    private static async Task<IResult> RemoverVeiculoAsync(
        string placa,
        IMediator mediator)
    {
        await mediator.Send(
            new RemoverVeiculoCommand(placa));

        return Results.NoContent();
    }
}