namespace GBastos.Casa_dos_Farelos.EstoqueService.Api.Endpoints.V1;

public static class EstoqueHealthEndpoints
{
    public static IEndpointRouteBuilder MapEstoqueHealth(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/health", () =>
        {
            return Results.Ok(new
            {
                Service = "EstoqueService",
                Status = "Healthy",
                Timestamp = DateTime.UtcNow
            });
        });

        return endpoints;
    }
}