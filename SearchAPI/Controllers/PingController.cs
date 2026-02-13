using Microsoft.AspNetCore.Mvc;

namespace SearchAPI.Controllers;

[ApiController]
[Route("ping")]
public class PingController : ControllerBase
{
    [HttpGet]
    public string? Ping()
    {
        return "SearchAPI - ver 1 - 13-02-2026";
    }
    
}