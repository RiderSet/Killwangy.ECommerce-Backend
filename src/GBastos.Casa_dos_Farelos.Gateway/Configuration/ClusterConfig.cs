namespace GBastos.Casa_dos_Farelos.Gateway.Configuration;

public sealed class ClusterConfig
{
    public Dictionary<string, DestinationConfig> Destinations { get; set; } = new();
}