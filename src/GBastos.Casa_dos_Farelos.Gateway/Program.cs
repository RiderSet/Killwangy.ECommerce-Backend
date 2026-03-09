using GBastos.Casa_dos_Farelos.Gateway.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddEndpointsApiExplorer();

#region API Versioning

services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;

    options.ReportApiVersions = true;

    options.ApiVersionReader =
        new UrlSegmentApiVersionReader();
});

services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

#endregion

#region Swagger

services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Casa dos Farelos Gateway",
        Version = "v1",
        Description = "API Gateway do sistema Casa dos Farelos"
    });

    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "Casa dos Farelos Gateway",
        Version = "v2",
        Description = "Versão 2 da API"
    });

    // JWT
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Authorization header usando Bearer",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",

        Reference = new OpenApiReference
        {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition("Bearer", jwtSecurityScheme);

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

#endregion

var app = builder.Build();

#region Swagger UI

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Gateway v1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "Gateway v2");

        options.RoutePrefix = "docs";

        options.DisplayRequestDuration();
        options.EnableTryItOutByDefault();
    });
}

#endregion

app.UseHttpsRedirection();

app.MapEventEndpoints();

app.Run()