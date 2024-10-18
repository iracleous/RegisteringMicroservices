namespace Consumer.Services;

using Consul;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

public class ConsulServiceDiscovery
{
    private readonly IConsulClient _consulClient;

    public ConsulServiceDiscovery(IConsulClient consulClient)
    {
        _consulClient = consulClient;
    }

    // Method to discover a service by name
    public async Task<Uri> DiscoverServiceAsync(string serviceName)
    {
        // Query Consul for the service based on the service name
        var services = await _consulClient.Agent.Services();
        var service = services.Response
            .Values
            .FirstOrDefault(s => s.Service.Equals(serviceName, StringComparison.OrdinalIgnoreCase));

        if (service == null)
        {
            throw new Exception($"Service '{serviceName}' not found in Consul.");
        }

        // Build the URI to the service (assuming HTTP for this example)
        var uri = new Uri($"http://{service.Address}:{service.Port}");
        return uri;
    }
}
