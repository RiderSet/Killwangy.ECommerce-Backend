using GBastos.Casa_dos_Farelos.EstoqueService.Application.Commands;
using GBastos.Casa_dos_Farelos.EstoqueService.Application.Enums;
using GBastos.Casa_dos_Farelos.EstoqueService.Application.Queries.Produtos;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Api.Endpoints.V1;

public static class ProdutoEndpoints
{
    public static void MapProdutoEndpointsV1(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/produtos").WithTags("Produtos");

        group.MapPost("/", CriarProduto);
        group.MapGet("/{id:guid}", ObterProduto);
        group.MapGet("/", ListarProdutos);
        group.MapPut("/{id:guid}", AtualizarProduto);
        group.MapDelete("/{id:guid}", RemoverProduto);

        group.MapPatch("/{id:guid}/ativar", AtivarProduto);
        group.MapPatch("/{id:guid}/desativar", DesativarProduto);

        group.MapPost("/{id:guid}/estoque/adicionar", AdicionarEstoque);
        group.MapPost("/{id:guid}/estoque/remover", RemoverEstoque);

        group.MapPost("/{id:guid}/reservar", ReservarProduto);
        group.MapPost("/reservas/{reservaId:guid}/confirmar", ConfirmarReserva);
        group.MapPost("/reservas/{reservaId:guid}/cancelar", CancelarReserva);
    }

    // ================= CRUD =================

    static async Task<IResult> CriarProduto(HttpContext http, CriarProdutoCommand cmd, IMediator mediator)
    {
        var idempotencyKey = http.Request.Headers.TryGetValue("Idempotency-Key", out var key) ? key.ToString() : null;

        var cmdComIdempotency = cmd with { IdempotencyKey = idempotencyKey };

        var result = await mediator.Send(cmdComIdempotency);

        return result.Success
            ? Results.Created($"/api/v1/produtos/{result.Data}", result)
            : Results.Problem(result.Error);
    }

    static async Task<IResult> ObterProduto(Guid id, IMediator mediator, IDistributedCache cache)
    {
        var cacheKey = $"produto:{id}";
        var cached = await cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cached))
            return Results.Content(cached, "application/json");

        var result = await mediator.Send(new ObterProdutoQuery(id));
        return result is not null ? Results.Ok(result) : Results.NotFound();
    }

    static async Task<IResult> ListarProdutos(int page, int pageSize, IMediator mediator)
    {
        var result = await mediator.Send(new ListarProdutosQuery(page, pageSize));
        return Results.Ok(result);
    }

    static async Task<IResult> AtualizarProduto(Guid id, AtualizarProdutoCommand cmd, IMediator mediator)
    {
        var command = cmd with { Id = id };

        var result = await mediator.Send(command);

        return result.Success
            ? Results.NoContent()
            : Results.Problem(result.Error);
    }

    static async Task<IResult> RemoverProduto(Guid id, IMediator mediator)
    {
        var result = await mediator.Send(new RemoverProdutoCommand(id));

        return result.Success
            ? Results.NoContent()
            : Results.Problem(result.Error);
    }

    // ================= STATUS =================

    static async Task<IResult> AtivarProduto(Guid id, IMediator mediator)
    {
        var result = await mediator.Send(new AtivarProdutoCommand(id));

        return result.Success
            ? Results.NoContent()
            : Results.Problem(result.Error);
    }

    static async Task<IResult> DesativarProduto(Guid id, IMediator mediator)
    {
        var result = await mediator.Send(new DesativarProdutoCommand(id));

        return result.Success
            ? Results.NoContent()
            : Results.Problem(result.Error);
    }

    // ================= ESTOQUE =================

    static async Task<IResult> AdicionarEstoque(Guid id, AjustarEstoqueCommand cmd, IMediator mediator)
    {
        var command = cmd with
        {
            ProdutoId = id,
            Tipo = TipoAjusteEstoque.Entrada
        };

        var result = await mediator.Send(command);

        return result.Success
            ? Results.Ok(result)
            : Results.Problem(result.Error);
    }

    static async Task<IResult> RemoverEstoque(Guid id, AjustarEstoqueCommand cmd, IMediator mediator)
    {
        var command = cmd with
        {
            ProdutoId = id,
            Tipo = TipoAjusteEstoque.Saida
        };

        var result = await mediator.Send(command);

        return result.Success
            ? Results.Ok(result)
            : Results.Problem(result.Error);
    }

    // ================= RESERVA =================

    static async Task<IResult> ReservarProduto(Guid id, ReservarProdutoCommand cmd, IMediator mediator)
    {
        var command = cmd with { ProdutoId = id };

        var result = await mediator.Send(command);

        return result.Success
            ? Results.Ok(result)
            : Results.Problem(result.Error);
    }

    static async Task<IResult> ConfirmarReserva(
        HttpContext http,
        Guid reservaId,
        IMediator mediator)
    {
        var key = http.Request.Headers["Idempotency-Key"].FirstOrDefault()
                  ?? Guid.NewGuid().ToString();

        var result = await mediator.Send(
            new ConfirmarReservaCommand(reservaId, key)
        );

        return result
            ? Results.NoContent()
            : Results.Problem("Erro ao confirmar reserva");
    }

    static async Task<IResult> CancelarReserva(Guid reservaId, IMediator mediator)
    {
        var result = await mediator.Send(
            new CancelarReservaCommand(reservaId)
        );

        return result.Success
            ? Results.NoContent()
            : Results.Problem(result.Error);
    }
}