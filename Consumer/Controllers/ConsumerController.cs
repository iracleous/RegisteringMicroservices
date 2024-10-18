using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Consumer.Controllers;

using Consumer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class ConsumerController : ControllerBase
{
    private readonly ConsulServiceDiscovery _serviceDiscovery;
    private readonly IHttpClientFactory _httpClientFactory;

    public ConsumerController(ConsulServiceDiscovery serviceDiscovery, IHttpClientFactory httpClientFactory)
    {
        _serviceDiscovery = serviceDiscovery;
        _httpClientFactory = httpClientFactory;
    }


    // Endpoint to demonstrate service discovery
    [HttpGet("consume")]
    public async Task<IActionResult> ConsumeService()
    {
        // Discover another service (e.g., "MyMicroservice")
        var serviceUri = await _serviceDiscovery.DiscoverServiceAsync("MyMicroservice");

        // Call the discovered service
        //   var response = await _httpClient.GetStringAsync(new Uri(serviceUri, "WeatherForecast"));
       //    return Ok(response);

        var client = _httpClientFactory.CreateClient();
        var uri = serviceUri +  "WeatherForecast";
        Console.WriteLine(uri);
        // Sending a GET request
        HttpResponseMessage response = await client.GetAsync(uri);

return Ok(response);


    }
}
