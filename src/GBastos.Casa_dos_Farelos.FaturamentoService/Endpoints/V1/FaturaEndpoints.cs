using GBastos.Casa_dos_Farelos.FaturamentoService.Application.Commands.CancelarFatura;
using GBastos.Casa_dos_Farelos.FaturamentoService.Application.Commands.EmitirFatura;
using GBastos.Casa_dos_Farelos.FaturamentoService.Application.Queries.ObtertFaturas;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Api.Endpoints.V1
{
    public static class FaturaEndpoints
    {
        public static IEndpointRouteBuilder MapFaturaEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/v1/faturas")
                           .WithTags("Faturas")
                           .RequireAuthorization();

            group.MapPost("/", CriarAsync);
            group.MapGet("/", ListarAsync);
            group.MapGet("/{id:guid}", ObterPorIdAsync);
            group.MapPut("/{id:guid}", AtualizarAsync);
            group.MapDelete("/{id:guid}", DeletarAsync);

            group.MapPost("/{id:guid}/cancelar", CancelarAsync);
            group.MapPost("/{id:guid}/pagar", MarcarComoPagaAsync);
            group.MapPost("/{id:guid}/vencer", MarcarComoVencidaAsync);

            return app;
        }

        private static async Task<IResult> CriarAsync(
            [FromBody] CriarFaturaCommand command,
            [FromServices] ISender sender)
        {
            var result = await sender.Send(command);
            return Results.Created($"/api/v1/faturas/{result}", result);
        }

        private static async Task<IResult> ListarAsync(
            [FromServices] ISender sender)
        {
            var query = new ListarFaturasQuery();
            var result = await sender.Send(query);
            return Results.Ok(result);
        }

        private static async Task<IResult> ObterPorIdAsync(
            Guid id,
            [FromServices] ISender sender)
        {
            var query = new ObterFaturaPorIdQuery(id);
            var result = await sender.Send(query);

            return result is null
                ? Results.NotFound()
                : Results.Ok(result);
        }

        private static async Task<IResult> AtualizarAsync(
            Guid id,
            [FromBody] AtualizarFaturaCommand command,
            [FromServices] ISender sender)
        {
            command.Id = id;
            await sender.Send(command);
            return Results.NoContent();
        }

        private static async Task<IResult> DeletarAsync(
            Guid id,
            [FromServices] ISender sender)
        {
            var command = new DeletarFaturaCommand(id);
            await sender.Send(command);
            return Results.NoContent();
        }

        private static async Task<IResult> CancelarAsync(
            Guid id,
            [FromServices] ISender sender)
        {
            var command = new CancelarFaturaCommand(id);
            await sender.Send(command);
            return Results.NoContent();
        }

        private static async Task<IResult> MarcarComoPagaAsync(
            Guid id,
            [FromServices] ISender sender)
        {
            var command = new MarcarFaturaComoPagaCommand(id);
            await sender.Send(command);
            return Results.NoContent();
        }

        private static async Task<IResult> MarcarComoVencidaAsync(
            Guid id,
            [FromServices] ISender sender)
        {
            var command = new MarcarFaturaComoVencidaCommand(id);
            await sender.Send(command);
            return Results.NoContent();
        }
    }
}