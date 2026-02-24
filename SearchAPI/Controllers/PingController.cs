using Microsoft.AspNetCore.Mvc;

namespace SearchAPI.Controllers;

[ApiController]
[Route("api/ping")]
public class PingController : ControllerBase
{
    [HttpGet]
    public string? Ping()
    {
        return Environment.GetEnvironmentVariable("id");
    }
    
}