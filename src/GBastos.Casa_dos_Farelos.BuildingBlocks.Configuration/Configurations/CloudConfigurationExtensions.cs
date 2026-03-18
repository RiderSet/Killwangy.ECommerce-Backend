using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace GBastos.Casa_dos_Farelos.BuildingBlocks.Configuration.Configurations;

public static class CloudConfigurationExtensions
{
    public static IConfigurationBuilder AddCloudConfiguration(
        this IConfigurationBuilder builder,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        var keyVaultUri = configuration["KeyVault:VaultUri"];

        if (!string.IsNullOrWhiteSpace(keyVaultUri))
        {
            builder.AddAzureKeyVault(
                new Uri(keyVaultUri),
                new DefaultAzureCredential());
        }

        var appConfigConnection = configuration["AzureAppConfiguration:ConnectionString"];

        if (!string.IsNullOrWhiteSpace(appConfigConnection))
        {
            builder.AddAzureAppConfiguration(appConfigConnection);
        }

        builder.AddEnvironmentVariables();

        return builder;
    }
}