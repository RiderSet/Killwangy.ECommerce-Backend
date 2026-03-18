using Azure.Identity;
using Microsoft.Extensions.Configuration;

namespace GBastos.Casa_dos_Farelos.BuildingBlocks.Configuration.Configurations;

public static class KeyVaultConfigurationExtensions
{
    public static IConfigurationBuilder AddKeyVaultSecrets(
        this IConfigurationBuilder builder,
        string keyVaultUri)
    {
        if (!string.IsNullOrWhiteSpace(keyVaultUri))
        {
            builder.AddAzureKeyVault(
                new Uri(keyVaultUri),
                new DefaultAzureCredential());
        }

        return builder;
    }
}