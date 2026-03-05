namespace GBastos.Casa_dos_Farelos.Gateway.Endpoints;

public static class GatewayHealthEndpoints
{
    public static IEndpointRouteBuilder MapGatewayHealth(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/health", () =>
        {
            return Results.Ok(new
            {
                Service = "Gateway",
                Status = "Healthy",
                Timestamp = DateTime.UtcNow
            });
        });

        return endpoints;
    }
}