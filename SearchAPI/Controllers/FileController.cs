using Microsoft.AspNetCore.Mvc;
namespace SearchAPI.Controllers;


[ApiController]
[Route("api/file")]
public class FileController : ControllerBase
{
    
    [HttpGet]
    [Route("get")]
    public string GetFile([FromQuery] string path)
    {
        var uri = "file://" + path;
        var s = System.IO.File.ReadAllText(path);
        return s;
    }
    
    
}
