using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Resistering.Controllers;

 

[Route("health")]
[ApiController]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Healthy");
    }
}
