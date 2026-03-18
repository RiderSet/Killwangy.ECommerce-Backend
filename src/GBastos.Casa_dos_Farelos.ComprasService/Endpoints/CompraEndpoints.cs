using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Common;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents.Persistence;
using GBastos.Casa_dos_Farelos.ComprasService.Api.Contract;
using GBastos.Casa_dos_Farelos.ComprasService.Api.Contracts;
using GBastos.Casa_dos_Farelos.ComprasService.Application.Interfaces;
using GBastos.Casa_dos_Farelos.ComprasService.Domain.Aggregates;

namespace GBastos.Casa_dos_Farelos.ComprasService.Api.Endpoints;

public static class CompraEndpoints
{
    public static IEndpointRouteBuilder MapCompraEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/compras")
            .WithTags("Compras");

        // Criar compra
        group.MapPost("/", async (
            CriarCompraRequest request,
            ICompraRepository repository,
            IUnitOfWork uow,
            CancellationToken ct) =>
        {
            var compra = Compra.CriarCompra(request.ClienteId);

            await repository.AddAsync(compra, ct);
            await uow.SaveChangesAsync(ct);

            return Results.Created($"/api/compras/{compra.Id}", new
            {
                compra.Id
            });
        });

        // Buscar compra
        group.MapGet("/{id:guid}", async (
            Guid id,
            ICompraRepository repository,
            CancellationToken ct) =>
        {
            var compra = await repository.GetByIdAsync(id, ct);

            return compra is null
                ? Results.NotFound()
                : Results.Ok(compra);
        });

        // Listar compras
        group.MapGet("/", async (
            ICompraRepository repository,
            CancellationToken ct) =>
        {
            var compras = await repository.ListAsync(ct);

            return Results.Ok(compras);
        });

        // Adicionar item
        group.MapPost("/{id:guid}/itens", async (
            Guid id,
            AdicionarItemRequest request,
            ICompraRepository repository,
            IUnitOfWork uow,
            CancellationToken ct) =>
        {
            var compra = await repository.GetByIdAsync(id, ct);

            if (compra is null)
                return Results.NotFound();

            var custo = new Money(request.CustoUnitario, "BRL");

            compra.AdicionarItem(
                request.ProdutoId,
                request.NomeProduto,
                request.Quantidade,
                custo);

            await uow.SaveChangesAsync(ct);

            return Results.NoContent();
        });

        // Alterar quantidade
        group.MapPut("/{compraId:guid}/itens/{itemId:guid}", async (
            Guid compraId,
            Guid itemId,
            AlterarQuantidadeRequest request,
            ICompraRepository repository,
            IUnitOfWork uow,
            CancellationToken ct) =>
        {
            var compra = await repository.GetByIdAsync(compraId, ct);

            if (compra is null)
                return Results.NotFound();

            compra.AlterarQuantidadeItem(itemId, request.Quantidade);
            repository.Update(compra);

            await uow.SaveChangesAsync(ct);

            return Results.NoContent();
        });

        // Finalizar compra
        group.MapPost("/{id:guid}/finalizar", async (
            Guid id,
            ICompraRepository repository,
            IUnitOfWork uow,
            CancellationToken ct) =>
        {
            var compra = await repository.GetByIdAsync(id, ct);

            if (compra is null)
                return Results.NotFound();

            compra.Finalizar();
            repository.Update(compra);
            await uow.SaveChangesAsync(ct);

            return Results.NoContent();
        });

        // Cancelar compra
        group.MapPost("/{id:guid}/cancelar", async (
            Guid id,
            ICompraRepository repository,
            IUnitOfWork uow,
            CancellationToken ct) =>
        {
            var compra = await repository.GetByIdAsync(id, ct);

            if (compra is null)
                return Results.NotFound();

            compra.Cancelar();
            repository.Update(compra);

            await uow.SaveChangesAsync(ct);

            return Results.NoContent();
        });

        return app;
    }
}