namespace GBastos.Casa_dos_Farelos.Gateway.Extensions;

public static class ApplicationBuilderExtensions
{
    public static WebApplication UseGatewayReverseProxy(
        this WebApplication app)
    {
        app.MapReverseProxy();
        return app;
    }
}