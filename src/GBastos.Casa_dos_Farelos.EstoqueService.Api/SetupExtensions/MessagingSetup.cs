using MassTransit;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Api.SetupExtensions;

public static class MessagingSetup
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumers(typeof(MessagingSetup).Assembly);

            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(ctx);
            });
        });

        return services;
    }
}