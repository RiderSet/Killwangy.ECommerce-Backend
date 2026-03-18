using Yarp.ReverseProxy.Configuration;

namespace GBastos.Casa_dos_Farelos.Gateway.Configuration;

public sealed class ReverseProxyConfig
{
    public Dictionary<string, RouteConfig> Routes { get; set; } = new();
    public Dictionary<string, ClusterConfig> Clusters { get; set; } = new();
}