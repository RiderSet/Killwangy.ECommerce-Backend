using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.DependencyInjections;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddMediatR(
            AppDomain.CurrentDomain.GetAssemblies());
        return services;
    }
}