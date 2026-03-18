using FluentValidation;
using GBastos.Casa_dos_Farelos.EstoqueService.Application.Behavior;
using MediatR;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Api.SetupExtensions;

public static class ApplicationSetup
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddMediatR(static cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ApplicationSetup).Assembly);
        });

        services.AddValidatorsFromAssembly(typeof(ApplicationSetup).Assembly);

        services.AddTransient(typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        return services;
    }
}