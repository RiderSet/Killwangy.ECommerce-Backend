using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Api.Swagger;

public sealed class CustomSchemaFilter : ISchemaFilter
{
    public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema is null)
            return;

        // Apenas exemplo simples seguro
        schema.Description ??= context.Type.Name;
    }
}